using System;
using System.Collections.Generic;
using System.Text;
using MyDemo.Bookstore.Localization;
using Volo.Abp.Application.Services;

namespace MyDemo.Bookstore;

/* Inherit your application services from this class.
 */
public abstract class BookstoreAppService : ApplicationService
{
    protected BookstoreAppService()
    {
        LocalizationResource = typeof(BookstoreResource);
    }
}
