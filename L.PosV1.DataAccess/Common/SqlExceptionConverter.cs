using NHibernate;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace L.PosV1.DataAccess.Common
{
    public class SqlExceptionConverter : ISQLExceptionConverter
    {
        public Exception Convert(AdoExceptionContextInfo exInfo)
        {
            var sqle = ADOExceptionHelper.ExtractDbException(exInfo.SqlException) as SqlException;
            if (sqle != null)
            {
                switch (sqle.Number)
                {
                    case 547:
                        return new ConstraintViolationException(exInfo.Message,
                            sqle.InnerException, exInfo.Sql, null);
                    case 208:
                        return new SQLGrammarException(exInfo.Message,
                            sqle.InnerException, exInfo.Sql);
                    case 3960:
                        return new StaleObjectStateException(exInfo.EntityName, exInfo.EntityId);
                }
            }
            return SQLStateConverter.HandledNonSpecificException(exInfo.SqlException,
                exInfo.Message, exInfo.Sql);
        }
    }
}
