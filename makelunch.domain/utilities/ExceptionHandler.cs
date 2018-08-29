using System;
using System.Net;
using System.Threading.Tasks;
using makeLunch.domain.utilities;
namespace makeLunch.domain.utilities
{
	public static class ExceptionHandler
	{
		/// <summary>
		/// Handles the exception asynchronous.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func">The function.</param>
		/// <returns></returns>
		public static async Task<Either<HttpStatusCodeErrorResponse, T>> HandleExceptionAsync<T>(Func<Task<T>> func)
		{
			var either = new EitherFactory<HttpStatusCodeErrorResponse, T>();
			// try
			// {
				return either.Create(await func().ConfigureAwait(false));
			//}
			// catch (ValidationException ex)
			// {
			// 	return either.Create(new HttpStatusCodeErrorResponse(HttpStatusCode.BadRequest, ex.Message));
			// }
			// catch (FullQueueException ex)
			// {
			// 	// todo: what should the HttpStatusCode be here?
			// 	return either.Create(new HttpStatusCodeErrorResponse(HttpStatusCode.BadRequest, ex.Message));
			// }
			// catch (NotFoundException ex)
			// {
			// 	return either.Create(new HttpStatusCodeErrorResponse(HttpStatusCode.NotFound, ex.Message));
			// }
			// catch (NotAuthorizedException ex)
			// {
			// 	return either.Create(new HttpStatusCodeErrorResponse(HttpStatusCode.Forbidden, ex.Message));
			// }
			// catch (InvalidCredentialsException)
			// {
			// 	return either.Create(new HttpStatusCodeErrorResponse(HttpStatusCode.BadRequest, "Invalid user credentials"));
			// }
			// catch (ConflictException ex)
			// {
			// 	return either.Create(new HttpStatusCodeErrorResponse(HttpStatusCode.Conflict, ex.Message));
			// }
		}
	}
}