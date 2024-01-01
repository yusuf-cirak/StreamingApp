global using AuthorizationFunctions =
    System.Collections.Generic.List<System.Func<Microsoft.AspNetCore.Http.HttpContext,
        System.Collections.Generic.ICollection<System.Security.Claims.Claim>,
        object,
        SharedKernel.Result>>;