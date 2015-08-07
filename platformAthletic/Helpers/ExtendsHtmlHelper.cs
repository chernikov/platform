using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Helpers
{
    /// <summary>
    /// Extends Razor HtmlHelper
    /// </summary>
    public static class ExtendsHtmlHelper
    {
        /// <summary>
        /// This method disable client side validation
        /// </summary>
        /// <param name="html"></param>
        /// <returns>class object implemets IDisposable</returns>
        public static ClientSideValidationDisabler BeginDisableClientSideValidation(this HtmlHelper html)
        {
            return new ClientSideValidationDisabler(html);
        }
    }

    /// <summary>
    /// A base class that implements IDisposable.
    /// </summary>
    public class ClientSideValidationDisabler : IDisposable
    {
        // Track whether Dispose has been called.
        private bool disposed = false;
        // Pointer to an external unmanaged resource.
        private HtmlHelper _html;

        public ClientSideValidationDisabler(HtmlHelper html)
        {
            _html = html;
            _html.EnableClientValidation(false);
        }

        public void Dispose()
        {

            this.Dispose(true);
            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    _html.EnableClientValidation(true);
                    _html = null;

                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.


                // Note disposing has been done.
                this.disposed = true;

            }
        }
    }
}