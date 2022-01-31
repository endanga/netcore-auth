
using System.Globalization;

namespace dotnet_api_project.Helpers;

public class AppExceptionc : Exception
{
    public AppExceptionc() : base(){}
    public AppExceptionc(string message) : base(message){}
    public AppExceptionc(string message, params object[] args) : base (String.Format(CultureInfo.CurrentCulture, message, args)){

    }
}