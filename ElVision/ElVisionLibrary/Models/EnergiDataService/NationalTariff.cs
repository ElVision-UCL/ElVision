using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.EnergiDataService
{
    public class NationalTariff
    {
        public NationalTariffType Type { get; set; }
        public double Price { get; set; }
    }

    public enum NationalTariffType
    {
        Elafgift,
        Systemtarif,
        TransmissionsNettarif
    }
}
