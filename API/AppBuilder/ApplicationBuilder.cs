namespace API.Builder
{
    public static class ApplicationBuilder
    {
        public static void AddBuilder(this IApplicationBuilder app, bool isDev)
        {

            if (isDev)
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.OAuthUsePkce();
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
        }

    }
}
