namespace bdNetCoreAPIDataTransfer
{
    public static class ApiRoutes
    {
        public static class ApiStartupRoute
        {
            public const string GetApiStartup = "api/start";
        }

        public static class ApiAuthenticationRoute
        {
            public const string PostWebLoginInformation = "authenticate/web/login";
            public const string PostApiLoginInformation = "authenticate/api/login";
        }

        public static class ApiTestRoute
        {
            public const string GetApiTestItems = "apitest/getall/";
            public const string GetApiTestItem = "apitest/get/{id}";
            public const string SaveApiTestItem = "apitest/save";
            public const string UpdateApiTestItem = "apitest/update";
            public const string DeleteApiTestItem = "apitest/delete";
        }

        public static class ExampleRoute
        {
            public const string GetExampleItems = "example/getall/";
            public const string GetExampleItem = "example/get/{id}";
            public const string SaveExampleItem = "example/save";
            public const string UpdateExampleItem = "example/update";
            public const string DeleteExampleItem = "example/delete";
        }
    }
}
