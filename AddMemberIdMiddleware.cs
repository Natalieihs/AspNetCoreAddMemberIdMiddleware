public class AddMemberIdMiddleware
{
    private readonly RequestDelegate _next;

    public AddMemberIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Invoke the next middleware in the pipeline
        await _next(context);

        // Only modify the response if it's successful and it's a JSON response
        if (context.Response.StatusCode == 200 && context.Response.ContentType == "application/json")
        {
            // Read the response body
            var responseBodyStream = context.Response.Body;
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();

            // Parse the response body as JSON
            var jObject = JObject.Parse(responseBody);

            // Add the memberid field to the response
            jObject.Add("memberid", 12345);

            // Convert the modified JSON object back to a string
            var modifiedResponseBody = jObject.ToString();

            // Write the modified response body back to the response
            context.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(modifiedResponseBody));
        }
    }
}
