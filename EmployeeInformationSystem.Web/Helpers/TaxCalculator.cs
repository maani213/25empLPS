using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Web.Helpers
{
    public class TaxCalculator
    {
        public static double CalculateTax(int salary)
        {
            double annualSalary = (salary * 12)*0.9;

            double annualTax = 0;
            double monthlyTax = 0;

            if (annualSalary <= 400000)
            {
                monthlyTax = 0;
            }

            if (annualSalary > 400000 && annualSalary <= 500000)
            {
                annualTax = (annualSalary - 400000) * 0.02;
                monthlyTax = annualTax / 12;
            }

            else if (annualSalary > 500000 && annualSalary <= 750000)
            {
                annualTax = ((annualSalary - 500000) * 0.05) + 2000;
                monthlyTax = (annualTax / 12);
            }

            else if (annualSalary > 750000 && annualSalary <= 1400000)
            {
                annualTax = ((annualSalary - 750000) * 0.1) + 14500;
                monthlyTax = annualTax / 12;
            }

            else if (annualSalary > 1400000 && annualSalary <= 1500000)
            {
                annualTax = ((annualSalary - 1400000) * 0.125) + 79500;
                monthlyTax = annualTax / 12;
            }

            else if (annualSalary > 1500000 && annualSalary <= 1800000)
            {
                annualTax = ((annualSalary - 1500000) * 0.15) + 92000;
                monthlyTax = annualTax / 12;
            }

            else if (annualSalary > 1800000 && annualSalary <= 2500000)
            {
                annualTax = ((annualSalary - 1800000) * 0.175) + 137000;
                monthlyTax = annualTax / 12;
            }

            else if (annualSalary > 2500000 && annualSalary <= 3000000)
            {
                annualTax = ((annualSalary - 2500000) * 0.2) + 259500;
                monthlyTax = annualTax / 12;
            }

            else if (annualSalary > 3000000 && annualSalary <= 3500000)
            {
                annualTax = ((annualSalary - 3000000) * 0.225) + 359500;
                monthlyTax = annualTax / 12;
            }

            else if (annualSalary > 3500000 && annualSalary <= 4000000)
            {
                annualTax = ((annualSalary - 3500000) * 0.25) + 472000;
                monthlyTax = annualTax / 12;
            }

            else if (annualSalary > 4000000 && annualSalary <= 7000000)
            {
                annualTax = ((annualSalary - 4000000) * 0.275) + 597000;
                monthlyTax = annualTax / 12;
            }

            return monthlyTax;
        }
    }
}
