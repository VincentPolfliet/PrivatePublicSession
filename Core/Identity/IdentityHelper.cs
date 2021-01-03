using System.Security.Principal;

namespace Core.Identity
{
	public static class IdentityHelper
	{
		public static bool IsRunAsAdmin()
		{
			// https://stackoverflow.com/questions/11660184/c-sharp-check-if-run-as-administrator#comment83080206_11660205
			using var identity = WindowsIdentity.GetCurrent();

			// https://stackoverflow.com/a/31856353
			return identity.Owner is not null && identity.Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
		}
	}
}
