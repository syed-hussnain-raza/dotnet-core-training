using Microsoft.AspNetCore.Identity;

namespace MyAssignment.Services
{
	/// <summary>
	/// Defines JWT token generation for authenticated Identity users.
	/// </summary>
	public interface IJwtTokenService
	{
		/// <summary>
		/// Generates a signed JWT for the given user, embedding their id, email, and role claims.
		/// </summary>
		string GenerateToken(Models.User user, IList<string> roles);
	}
}