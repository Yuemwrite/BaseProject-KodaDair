using System.Data;
using Microsoft.Data.SqlClient;

namespace BaseProject.Application.Common.Extensions;


public class SqlQueryWrapper
{
    public List<SqlParameter> Parameters { get; set; }
    public string Query { get; set; }
}

public static class DbFunctionExtensions
{
    public static SqlQueryWrapper ToSqlQueryAndParameters(this string procedureName, string? schemeName = "dbo", Dictionary<string, object> procedureParameters = null)
    {
        var schemePart = schemeName == null ? string.Empty : $"[{schemeName ?? string.Empty}].";
        var sqlString = $"EXEC {schemePart}[{procedureName}] ";
        var parameters = new List<SqlParameter>();

        if (procedureParameters != null && procedureParameters.Count > 0)
        {
            foreach (var procParam in procedureParameters)
            {
                if (procParam.Value is IDbDataParameter)
                {
                    var oldParameter = (SqlParameter)procParam.Value;
                    var newParameter = new SqlParameter(oldParameter.ParameterName, oldParameter.Value)
                    {
                        SqlDbType = oldParameter.SqlDbType,
                        TypeName = oldParameter.TypeName
                    };

                    parameters.Add(newParameter);
                }
                else if (procParam.Value is DateTime || procParam.Value is Nullable<DateTime>)
                    parameters.Add(new SqlParameter(procParam.Key, procParam.Value ?? DBNull.Value)
                    {
                        SqlDbType = SqlDbType.DateTime2
                    });
                else
                    parameters.Add(new SqlParameter(procParam.Key, procParam.Value ?? DBNull.Value));

                sqlString += $"@{procParam.Key}=@{procParam.Key}, ";
            }
        }

        sqlString = sqlString.Trim().TrimEnd(',');

        return new SqlQueryWrapper
        {
            Parameters = parameters,
            Query = sqlString
        };
    }
}