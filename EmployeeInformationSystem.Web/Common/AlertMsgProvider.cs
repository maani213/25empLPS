using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeInformationSystem.Web.Common
{
    public class AlertMessageProvider
    {
        public static string SuccessMessage(string msg)
        {
            string alert = "<div id='alert-message' role = 'alert' class='alert alert-contrast alert-success alert-dismissible'>" +
                                             "<div class='icon'><span class='mdi mdi-check'></span></div>" +
                                             "<div class='message'>" +
                                                   "<button type = 'button' data-dismiss='alert' aria-label='Close' class='close'><span aria-hidden='true' class='mdi mdi-close'></span></button><strong>Success!</strong> " + msg +
                                             "</div>" +
                                        "</div>";
            return alert;
        }

        public static string FailureMessage(string msg)
        {
            string alert = "<div id='alert-message' role = 'alert' class='alert alert-contrast alert-danger alert-dismissible'>" +
                                            "<div class='icon'><span class='mdi mdi-check'></span></div>" +
                                            "<div class='message'>" +
                                                  "<button type = 'button' data-dismiss='alert' aria-label='Close' class='close'><span aria-hidden='true' class='mdi mdi-close'></span></button><strong>Failure!</strong> " + msg +
                                            "</div>" +
                                       "</div>";
            return alert;
        }
    }
}