using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newbtc
{
    public class ModelInput
    {
        
        [LoadColumn(0)]
        public float Price { get; set; }
        
    }
    
}
