using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServiceWCF.Helpers
{
    /// <summary>
    /// Validate date, only +18
    /// </summary>
    public class ValidationDate : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            return (int)(DateTime.Now.Subtract((DateTime)value).TotalDays / 365) >= Constants.AGE;
        }
    }
}
