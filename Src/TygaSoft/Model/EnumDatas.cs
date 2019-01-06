namespace TygaSoft.Model
{
    public enum FunFlagOptions
    {
        None,
        Orders,
        OrderPackages,
        OrderBack
    }

    public enum OrderStatusOptions
    {
        None,
        Send,
        Package,
        End=99
    }

    public enum OrderRoleOptions
    {
        None,
        Administrator,
        System,
        Users,
        OrderSended,
        OrderPackaged,
        OrderEnded
    }

    public enum ApplicationOptions
    {
        None = 100000
    }
    public enum ResCodeOptions
    {
        Success = 1000,
        Error = 1001,
        TokenInvalidError = 1002
    }

    public enum PasswordFormatOptions { Clear, Hashed, Aes, Sha1, Sha256, Md5 };

    public enum PayOptions
    {

    }

    public enum PurposeOptions
    {
        ADULT
    }

    public enum ParameterOptions
    {
        Cookie,
        GetOrPost,
        UrlSegment,
        HttpHeader,
        HttpContentHeader,
        RequestBody,
        QueryString,
        FormUrlEncodedContent
    }

    public enum HttpMethodOptions
    {
        Get,
        Post,
        Put,
        Delete,
        Head,
        Options,
        Patch,
        Merge,
        Copy
    }
}