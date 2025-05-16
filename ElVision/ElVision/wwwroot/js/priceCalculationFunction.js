function isUndefined(value) {return typeof value === "undefined";}
function calculateFakeProductPrice(validdate, calculateTax, userSelection, consumption, distributionAreaCharge, nordpool, nationalCharges) {
  //
  // Calculate the total price for a "market product", which have a price that is 0,02 øre above Nord Pool spot price
  //
  var fakeProduct = {
    "deliveryPeriodDays": 180,
    "billingType": "fix",
    "fees": [],
    "productPrices": [{
      "subscription": 0,
      "containsNetworkTariff": false,
      "additionalNordPoolSpot": true,
      "matrix": [{
        "hoursFrom": 0,
        "hoursTo": 24,
        "volumeFrom": 0,
        "volumeTo": 999999,
        "amount": 0.02
      }]
    }]
  };
  return calculatePrice(validdate, calculateTax, userSelection, consumption, fakeProduct, null, distributionAreaCharge, nordpool, nationalCharges);
}
function between(x, min, max) {
  if (isUndefined(min) || min === null)
    min = x;
  if (isUndefined(max) || max === null)
    max = x;
  return x >= min && x <= max;
}
function calculatePrice(validdate, calculateTax, userSelection, consumption, product, productNext, distributionAreaCharge, nordpool, nationalCharges) {
  // Pre-conditions:
  // validdate is not used anymore.
  //
  // calculateTax is a boolean to indicate whether tax should be added or not
  //
  //
  // consumption is the consumption in procent per hour of week.
  //
  // product contains one productPrice that is valid for the given date.
  // distributionAreaCharge is valid for the given date
  //
  // nordpool is an array with last month (compared to given date) average per hour for both DK1 and DK2
  //
  // nationalCharges contains fees/tariffs for the whole country and valid for the given date
  // profile contains the total usage as well as usageCode.
  // UsageCode comes from the subProfile. UsageCode can be null, "electric", "renewable" or "own". If no usage is given, default is 4000.
  var profile = userSelection.profile;
  var powerConsumption = userSelection.powerConsumption !== null ? parseInt(userSelection.powerConsumption) : profile.usage;
  if (calculateTax === null) {
    calculateTax = true;
  }
  // Get the yearly usage and usageCode (if any)
  var annualConsumption = powerConsumption;
  if (annualConsumption === null || isNaN(annualConsumption)) {
    annualConsumption = 4000;
  }
  // electrical heating
  var electricalHeating = false;
  if (profile.usageCode !== null && profile.usageCode === "electric") {
    electricalHeating = true;
  }
  var deliveryDays = product.deliveryPeriodDays > 180 ? 180 : product.deliveryPeriodDays;
  var productIsExpiring = false;
  var product1 = calculatePriceDetail(deliveryDays, calculateTax, annualConsumption, electricalHeating, consumption, product, distributionAreaCharge, nordpool, nationalCharges);
  if (product1 === null) { // if the productprice is based on nordpool prices and no nordpool is given, product1 will be null
    return;
  }
  var product2 = {
    "totalPrice": 0,
    "totalTax": 0,
    "supplierPrice": 0,
    "gridCompanyPrice": 0,
    "statePrice": 0,
    "productPriceAmount": 0,
    "productSubscription": 0,
    "productFee": 0,
    "distributionTariff": 0,
    "distributionSubscription": 0,
    "distributionFee": 0,
    "transmissionTariff": 0,
    "systemTariff": 0,
    "PSOTarif": 0,
    "elAfgift": 0,
    "elAfgiftReduceret": 0,
    "isNetworktariffIncluded": false
  };
  var restDays = 0;
  if (product.deliveryPeriodDays < 180 && productNext) {
    restDays = 180 - deliveryDays;
    product2 = calculatePriceDetail(restDays, calculateTax, annualConsumption, electricalHeating, consumption, productNext, distributionAreaCharge, nordpool, nationalCharges);
    if (product2 === null) { // if the productprice is based on nordpool prices and no nordpool is given, product2 will be null.
      return;
    }
  } else if (product.deliveryPeriodDays < 180 && !productNext) {
    //Product is expiring within 180 days. Postpone so we can calculate a more accurate price
    deliveryDays = 180;
    productIsExpiring = true;
  }
  var totalIncludingTaxPerKWH = ((product1.totalPrice + product1.totalTax) * deliveryDays / 180 + (product2.totalPrice + product2.totalTax) * restDays / 180) / annualConsumption;
  var totalIncludingTax = ((product1.totalPrice + product1.totalTax) * deliveryDays / 180 + (product2.totalPrice + product2.totalTax) * restDays / 180) / 100 / 2; // shown in kr, amount for 6 months
  var totalTax = (product1.totalTax * deliveryDays / 180 + product2.totalTax * restDays / 180) / annualConsumption;
  var supplier = (product1.supplierPrice * deliveryDays / 180 + product2.supplierPrice * restDays / 180) / annualConsumption;
  var supplierIncludingTax = supplier * 1.25;
  var gridCompany = (product1.gridCompanyPrice * deliveryDays / 180 + product2.gridCompanyPrice * restDays / 180) / annualConsumption;
  var state = (product1.statePrice * deliveryDays / 180 + product2.statePrice * restDays / 180) / annualConsumption;
  //
  // OUTPUT
  //
  var calculatedPrice = {};
  calculatedPrice["productIsExpiring"] = productIsExpiring;
  calculatedPrice["totalPriceSortable"] = totalIncludingTaxPerKWH; // For sorting
  //console.log("Full PR price " +totalIncludingTaxPerKWH );
  calculatedPrice["totalPrice"] = prettify(totalIncludingTaxPerKWH) + " øre/kWh"; // pris per kWh
  calculatedPrice["deliveryDaysPrice"] = prettify(totalIncludingTax) + " kr"; // pris per 6 måneder (normalt)
  calculatedPrice["supplierPriceIncludingTax"] = prettify(supplierIncludingTax) + " øre"; // pris til el-leverandør inkl. moms
  calculatedPrice["supplierPrice"] = prettify(supplier) + " øre"; // pris til el-leverandør ex moms
  calculatedPrice["gridCompanyPrice"] = prettify(gridCompany) + " øre"; // pris til gridCompanyPrice
  calculatedPrice["statePrice"] = prettify(state) + " øre"; // pris til staten
  calculatedPrice["prices"] = [];
  var productPrice = (product1.productPriceAmount * deliveryDays / 180 + product2.productPriceAmount * restDays / 180) / annualConsumption;
  var productSubscription = (product1.productSubscription * deliveryDays / 180 + product2.productSubscription * restDays / 180) / annualConsumption;
  var productFee = (product1.productFee * deliveryDays / 180 + product2.productFee * restDays / 180) / annualConsumption;
  var tariffDistribution = 0;
  var subscriptionDistribution = 0;
  var feeDistribution = 0;
  var tariffTransmission = 0;
  var tariffSystem = 0;
  var tariffPSO = (product1.PSOTarif * deliveryDays / 180 + product2.PSOTarif * restDays / 180) / annualConsumption;
  var feeEl = (product1.elAfgift * deliveryDays / 180 + product2.elAfgift * restDays / 180) / annualConsumption;
  var feeElReduced = (product1.elAfgiftReduceret * deliveryDays / 180 + product2.elAfgiftReduceret * restDays / 180) / annualConsumption; // only for electrical heating, with annualConsumption above 4000
  if (!product1.isNetworktariffIncluded) {
    tariffDistribution = product1.distributionTariff * deliveryDays / 180 / annualConsumption;
    subscriptionDistribution = product1.distributionSubscription * deliveryDays / 180 / annualConsumption;
    feeDistribution = product1.distributionFee * deliveryDays / 180 / annualConsumption;
    tariffTransmission = product1.transmissionTariff * deliveryDays / 180 / annualConsumption;
    tariffSystem = product1.systemTariff * deliveryDays / 180 / annualConsumption;
  }
  if (!product2.isNetworktariffIncluded) {
    tariffDistribution = tariffDistribution + product2.distributionTariff * restDays / 180 / annualConsumption;
    subscriptionDistribution = subscriptionDistribution + product2.distributionSubscription * restDays / 180 / annualConsumption;
    feeDistribution = feeDistribution + product2.distributionFee * restDays / 180 / annualConsumption;
    tariffTransmission = tariffTransmission + product2.transmissionTariff * restDays / 180 / annualConsumption;
    tariffSystem = tariffSystem + product2.systemTariff * restDays / 180 / annualConsumption;
  }
  var subPrices = [];
  var asterix = "";
  if (product1.isNetworktariffIncluded || (product2.isNetworktariffIncluded)) {
    calculatedPrice.prices.push({
      "name": "* Priserne for el inkluderer betaling for transport",
      "amountText": null
    });
    asterix = " *";
  }
  subPrices = [{
    "id": 1,
    "name": "  El",
    "amountText": "     " + prettify(productPrice) + " øre/kWh" + asterix,
    "tooltipKey": "el"
  },
    {
      "id": 2,
      "name": "  El (abonnement)",
      "amountText": "     " + prettify(productSubscription) + " øre/kWh",
      "tooltipKey": "elSubscription"
    },
    {
      "id": 3,
      "name": "  El (gebyr)",
      "amountText": "     " + prettify(productFee) + " øre/kWh",
      "tooltipKey": "elFee"
    }
  ];
  calculatedPrice.prices.push({
    "name": "Betaling for el",
    "amountText": prettify(supplier) + " øre/kWh",
    "subPrices": subPrices,
    "tooltipKey": "elBetaling"
  });
  subPrices = [{
    "id": 1,
    "name": "  El-transport",
    "amountText": prettify(tariffDistribution + tariffTransmission + tariffSystem) + " øre/kWh",
    "tooltipKey": "elTariffs"
  },
    {
      "id": 2,
      "name": "  El-transport (abonnement)",
      "amountText": prettify(subscriptionDistribution) + " øre/kWh",
      "tooltipKey": "elDistributionSubscription"
    }
  ];
  calculatedPrice.prices.push({
    "name": "Betaling for transport",
    "amountText": prettify(gridCompany) + " øre/kWh",
    "subPrices": subPrices,
    "tooltipKey": "elTransport"
  });
  subPrices = [{
    "id": 3,
    "name": "  PSO-afgift",
    "amountText": prettify(tariffPSO) + " øre/kWh",
    "tooltipKey": "elPSOTariff"
  },
    {
      "id": 4,
      "name": "  Beregnet el-afgift",
      "amountText": prettify(feeEl) + " øre/kWh",
      "tooltipKey": "elStateFee"
    }
  ];
  if (feeElReduced > 0) {
    subPrices.push({
      "id": 5,
      "name": "  Beregnet reduceret El-afgift",
      "amountText": prettify(feeElReduced) + " øre/kWh",
      "tooltipKey": "elReducedStateFee"
    });
  }
  calculatedPrice.prices.push({
    "name": "Afgifter",
    "amountText": prettify(state) + " øre/kWh",
    "subPrices": subPrices,
    "tooltipKey": "elGebyrer"
  });
  if (calculateTax) {
    calculatedPrice.prices.push({
      "name": "Moms (25,00 pct.)",
      "amountText": prettify(totalTax) + " øre/kWh",
      "tooltipKey": "elMoms"
    });
  }
  return calculatedPrice;
}
function calculatePriceDetail(deliveryPeriodDays, calculateTax, annualConsumption, electricalHeating, consumption, product, distributionAreaCharge, nordpool, nationalCharges) {
  //
  // CONSUMPTION
  // Build the hour-per-hour compsumption array (in percentage). If no compsumption is given, split the 100 % evenly on to all hours.
  //
  var consumptionProfilePercentage = [];
  if (consumption.hourProfile !== undefined &&
    consumption.hourProfile["1"] && consumption.hourProfile["1"].length &&
    consumption.hourProfile["2"] && consumption.hourProfile["2"].length &&
    consumption.hourProfile["3"] && consumption.hourProfile["3"].length &&
    consumption.hourProfile["4"] && consumption.hourProfile["4"].length &&
    consumption.hourProfile["5"] && consumption.hourProfile["5"].length &&
    consumption.hourProfile["6"] && consumption.hourProfile["6"].length &&
    consumption.hourProfile["7"] && consumption.hourProfile["7"].length) {
    for (var dayKey in consumption.hourProfile) {
      if (consumption.hourProfile.hasOwnProperty(dayKey)) {
        var parsedDay = parseInt(dayKey);
        if ((parsedDay > 0) && (parsedDay < 8)) {
          consumptionProfilePercentage[parsedDay] = [24]
          for (var i = 0; i < consumption.hourProfile[dayKey].length; i++) {
            // find the consumption in percent per hour
            for (var h = consumption.hourProfile[dayKey][i].hourFrom; h < consumption.hourProfile[dayKey][i].hourTo; h++) {
              var hourProcent = consumption.hourProfile[dayKey][i].percentagePerHour;
              consumptionProfilePercentage[parsedDay][h] = hourProcent;
            }
          }
        }
      }
    }
  } else {
    for (var d = 1; d <= 7; d++) {
      consumptionProfilePercentage[d] = [24]
      for (var h = 0; h < 24; h++) {
        consumptionProfilePercentage[d][h] = 100 / 24 / 7;
      }
    }
  }
  //Select the product price which matches end users annual consumption. (CR #616)
  var validProductPrice = null;
  if (product.productPrices !== undefined) {
    for (var i = 0, len = product.productPrices.length; i < len; i++) {
      var productPrice = product.productPrices[i];
      if (between(annualConsumption, productPrice.minimumConsumption, productPrice.maximumConsumption)) {
        validProductPrice = productPrice;
        break;
      }
    }
  } else if (product.productPrice !== undefined) {
    validProductPrice = product.productPrice;
  }
  if (!validProductPrice) {
    return null;
  }
  //
  // PRODUCT PRICE
  // Loop thru all the lines in the productPriceMatrix and calculate the price for the given volumen and hours.
  // Sum all the prices to get the total productprice for the year
  //
  var subPrices = []; // defined here because of error in minifying. Used in OUTPUT section
  // Set default values
  var isFixBilling = true;
  var isNetworktariffIncluded = false;
  var annualProductSubscription = 0;
  var monthlyProductSubscription = 0;
  var annualProductPrice = 0;
  isFixBilling = product.billingType === "fix";
  isNetworktariffIncluded = validProductPrice.containsNetworkTariff;
  monthlyProductSubscription = validProductPrice.subscription;
  annualProductSubscription = monthlyProductSubscription * 12;
  //
  // Build the hour-per-hour nordpoolProfile based on the nordpool object. Use "DK1" as default.
  // If no nordpool spot prices are needed, set all hours to 0.
  //
  var nordpoolProfile = [24];
  if (validProductPrice.additionalNordPoolSpot) {
    if (!nordpool) { // if the productprice is based on nordpool prices and no nordpool is given, return null.
      alert("nordpool prices must be given");
      return null;
    }
    var nordpools = nordpool.nordpools[0];
    if (distributionAreaCharge.area === nordpool.nordpools[1].area) {
      nordpools = nordpool.nordpools[1];
    }
    for (var h = 0; h < 24; h++) {
      nordpoolProfile[h] = nordpools.hourProcent[h];
    }
  } else { // if no add on of nordpoolspot, set array to 0
    for (var h = 0; h < 24; h++) {
      nordpoolProfile[h] = 0;
    }
  }
  //
  // calculate the price per volume interval and hour interval
  //
  for (var day = 1; day <= 7; day++) {
    // by default use the original matrix;
    var matrix = validProductPrice.matrix;
    // but if other days exist for the given day, use matrix from there
    if (validProductPrice.otherDays && validProductPrice.otherDays.length) {
      for (var od in validProductPrice.otherDays) {
        for (var d = 0; d < validProductPrice.otherDays[od].days.length; d++) {
          if (validProductPrice.otherDays[od].days[d] === day) {
            matrix = validProductPrice.otherDays[od].matrix;
          }
        }
      }
    }
    for (var j = 0; j < matrix.length; j++) {
      var thisVolumeFrom = matrix[j].volumeFrom;
      var thisVolumeTo = matrix[j].volumeTo;
      var thisHoursFrom = matrix[j].hoursFrom;
      var thisHoursTo = matrix[j].hoursTo;
      var thisAmount = matrix[j].amount;
      var volume = 0;
      var price = 0;
      // calculate the volume to use for this product price line
      if (annualConsumption > thisVolumeTo) {
        volume = thisVolumeTo - thisVolumeFrom;
      } else {
        if (annualConsumption < thisVolumeFrom) {
          continue;
        } else {
          volume = annualConsumption - thisVolumeFrom;
        }
      }
      //
      // find the product price for the whole year
      //
      //!NB nordpoolProfile[] values area in dkk øre per MWh
      if (isFixBilling) {
        var nordpoolSum = 0.0;
        var productMatrixSum = 0.0;
        for (var h = thisHoursFrom; h < thisHoursTo; h++) {
          nordpoolSum += nordpoolProfile[h] * 10;
          productMatrixSum += thisAmount;
        }
        var nordpoolAverage = nordpoolSum / 24 / 7;
        var productMatrixAverage = productMatrixSum / 24 / 7;
        annualProductPrice = annualProductPrice + (productMatrixAverage + nordpoolAverage) * volume;
      } else {
        for (var h = thisHoursFrom; h < thisHoursTo; h++) {
          price = (thisAmount + nordpoolProfile[h] * 10) * volume * consumptionProfilePercentage[day][h] / 100;
          annualProductPrice = annualProductPrice + price;
        }
      }
    }
  }
  //
  // PRODUCT FEE
  // Calculate the total annual fee for the product
  //
  var annualProductFee = 0;
  for (var i = 0; i < product.fees.length; i++) {
    if (product.fees[i].tooltipKey !== "feeSupplierChange" && product.fees[i].tooltipKey !== "feeProductChange") {
      annualProductFee = annualProductFee + (product.fees[i].amount);
    }
  }
  //
  // NETWORK TARIFF / DISTRIBUTION_AREA_CHARGE
  // Calculate the annual distribution expenses; subscription, tariff(fix or flex) (and fee)
  //
  var annualDistributionSubscription = 0;
  var monthlyDistributionSubscription = 0;
  var annualDistributionTariff = 0;
  var annualDistributionFee = 0;
  var distributionChargeAreaTariff = 0;
  var networkTariffFlexExists = false;
  //first calculate distributionarea charge for from flex
  if (!isNetworktariffIncluded) {
    for (var i = 0; i < distributionAreaCharge.distributionAreaCharges.length; i++) {
      if (distributionAreaCharge.distributionAreaCharges[i].billingType === "flex"){
       if (distributionAreaCharge.distributionAreaCharges[i].chargeType === "tariff") {
        networkTariffFlexExists = true;
        for (var j = 0; j < distributionAreaCharge.distributionAreaCharges[i].distributionAreaChargeHours.length; j++) {
           for (var day = 1; day <= 7; day++) {
             for (var h = distributionAreaCharge.distributionAreaCharges[i].distributionAreaChargeHours[j].hoursFrom; h < distributionAreaCharge.distributionAreaCharges[i].distributionAreaChargeHours[j].hoursTo; h++) {
               distributionChargeAreaTariff += (annualConsumption * consumptionProfilePercentage[day][h] / 100 * distributionAreaCharge.distributionAreaCharges[i].distributionAreaChargeHours[j].amount);
             }
           }
         }
       }
      }
    }
  }
  //console.log("Dist. Charge =" +distributionChargeAreaTariff );
  // check if is_Networktariff_Included (= is false)
  if (!isNetworktariffIncluded) {
    if (networkTariffFlexExists) { // Flex tariff
      for (var i = 0; i < distributionAreaCharge.distributionAreaCharges.length; i++) {
        if (distributionAreaCharge.distributionAreaCharges[i].billingType === "flex") {
            if (distributionAreaCharge.distributionAreaCharges[i].chargeType === "subscription") {
              monthlyDistributionSubscription += distributionAreaCharge.distributionAreaCharges[i].price;
            } 
            else if (distributionAreaCharge.distributionAreaCharges[i].chargeType === "tariff") {
              //just assign already calculated distributiion ara charge
              annualDistributionTariff = distributionChargeAreaTariff;
              //console.log("Annual flex nettarif: " + annualDistributionTariff);
            }
            else { // fee
              annualDistributionFee += distributionAreaCharge.distributionAreaCharges[i].price * annualConsumption;
            }
        } 
      }
    }
    else { // Fix tariff
      for (var i = 0; i < distributionAreaCharge.distributionAreaCharges.length; i++) {
        if (distributionAreaCharge.distributionAreaCharges[i].billingType === "fix") {
          if (distributionAreaCharge.distributionAreaCharges[i].chargeType === "subscription") {
            monthlyDistributionSubscription += distributionAreaCharge.distributionAreaCharges[i].price;
          } else if (distributionAreaCharge.distributionAreaCharges[i].chargeType === "tariff") {
            annualDistributionTariff += distributionAreaCharge.distributionAreaCharges[i].price * annualConsumption;
            //console.log("Annual fix nettarif: " + annualDistributionTariff);
          } else { // fee
            annualDistributionFee += distributionAreaCharge.distributionAreaCharges[i].price * annualConsumption;
          }
        }
      }
    }
  }
  //Distribution area subscription price is received as kr/month. Calculate annual subscription price.
  annualDistributionSubscription = monthlyDistributionSubscription * 12;
  //
  // NATIONAL CHARGEs
  // Calculate the annual state expenses; transmission tariff, system tariff, PSO tariff, el-afgift
  //
  var transmissionTariff = 0;
  var systemTariff = 0;
  var psoTariff = 0;
  var elAfgift = 0;
  var reduceretElAfgift = 0;
  for (var i = 0; i < nationalCharges.nationalCharges.length; i++) {
    if (nationalCharges.nationalCharges[i].chargeType === "transmissionTariff") {
      transmissionTariff = nationalCharges.nationalCharges[i].amount;
    }
    if (nationalCharges.nationalCharges[i].chargeType === "systemTariff") {
      systemTariff = nationalCharges.nationalCharges[i].amount;
    }
    if (nationalCharges.nationalCharges[i].chargeType === "psoTariff") {
      psoTariff = nationalCharges.nationalCharges[i].amount;
    }
    if (nationalCharges.nationalCharges[i].chargeType === "elAfgift") {
      elAfgift = nationalCharges.nationalCharges[i].amount;
    }
    if (nationalCharges.nationalCharges[i].chargeType === "reduceretElAfgift") {
      reduceretElAfgift = nationalCharges.nationalCharges[i].amount;
    }
  }
  // setting default values
  var annualTransmissionTariff = 0;
  var annualSystemTariff = 0;
  var annualPSOTarif = psoTariff * annualConsumption;
  var annualElAfgift = 0;
  var annualElAfgiftReduceret = 0;
  if (!isNetworktariffIncluded) {
    annualTransmissionTariff = transmissionTariff * annualConsumption; // opgivet i kr/kWh
    annualSystemTariff = systemTariff * annualConsumption; // opgivet i kr/kWh
  }
  if (electricalHeating) {
    if (annualConsumption > 4000) {
      annualElAfgift = elAfgift * 4000;
      annualElAfgiftReduceret = reduceretElAfgift * (annualConsumption - 4000);
    } else {
      annualElAfgift = elAfgift * annualConsumption;
    }
  } else {
    annualElAfgift = elAfgift * annualConsumption;
  }
  // Calculate the shares of the product price that will be payed to the supplier, the gridcompany and the state, for the given product
  var calculatedSupplierPrice = annualProductPrice + 100 * (annualProductSubscription + annualProductFee);
  var calculatedGridCompanyPrice = 100 * (annualSystemTariff + annualDistributionTariff + annualTransmissionTariff + annualDistributionSubscription + annualDistributionFee);
  var calculatedStatePrice = 100 * (annualPSOTarif + annualElAfgift + annualElAfgiftReduceret);
  //
  // TOTAL PRICE AND TOTAL TAX
  // Calculate the total productprice and a possible tax
  //
  var totalPrice = (calculatedSupplierPrice + calculatedGridCompanyPrice + calculatedStatePrice);
  //console.log("Total Price " + totalPrice)
  var totalTax = 0;
  if (calculateTax) {
    totalTax = (totalPrice * 0.25);
    monthlyProductSubscription = monthlyProductSubscription * 1.25;
    monthlyDistributionSubscription = monthlyDistributionSubscription * 1.25;
  }
  var totalMonthlySubscription = monthlyProductSubscription + (isNetworktariffIncluded ? 0.00 : monthlyDistributionSubscription);
  product["totalMonthlySubscription"] = prettify(totalMonthlySubscription) + " kr. " + (calculateTax ? " (inkl. moms)" : " (ekskl. moms)");
  product["monthlyProductSubscription"] = prettify(monthlyProductSubscription) + " kr. " + (calculateTax ? " (inkl. moms)" : " (ekskl. moms)");
  product["monthlyDistributionSubscription"] = prettify(monthlyDistributionSubscription) + " kr. " + (calculateTax ? " (inkl. moms)" : " (ekskl. moms)");
  product["isNetworktariffIncluded"] = isNetworktariffIncluded;
  //
  // CALCULATIONS
  // Convert all amounts to øre and return all the calculated prices
  //
  product["totalPrice"] = totalPrice;
  product["totalTax"] = totalTax;
  product["supplierPrice"] = calculatedSupplierPrice;
  product["gridCompanyPrice"] = calculatedGridCompanyPrice;
  product["statePrice"] = calculatedStatePrice;
  product["productPriceAmount"] = annualProductPrice;
  product["productSubscription"] = annualProductSubscription * 100;
  product["productFee"] = annualProductFee * 100;
  product["distributionTariff"] = annualDistributionTariff * 100;
  product["distributionSubscription"] = annualDistributionSubscription * 100;
  product["distributionFee"] = annualDistributionFee * 100;
  product["transmissionTariff"] = annualTransmissionTariff * 100;
  product["systemTariff"] = annualSystemTariff * 100;
  product["PSOTarif"] = annualPSOTarif * 100;
  product["elAfgift"] = annualElAfgift * 100;
  product["elAfgiftReduceret"] = annualElAfgiftReduceret * 100;
  product["isNetworktariffIncluded"] = isNetworktariffIncluded;
  return product;
}