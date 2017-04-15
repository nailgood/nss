using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maxmind.Entities
{
    public enum ErrorReporting
    {
        INVALID_LICENSE_KEY, IP_REQUIRED,
        IP_NOT_FOUND, MAX_REQUESTS_REACHED, LICENSE_REQUIRED, COUNTRY_NOT_FOUND
            , CITY_NOT_FOUND, CITY_REQUIRED, INVALID_EMAIL_MD5, POSTAL_CODE_REQUIRED, POSTAL_CODE_NOT_FOUND
    }
}
