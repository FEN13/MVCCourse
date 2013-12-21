namespace PhotoGalery.Services
{
	public static class Enums
	{
		public enum UserResetPasswordStatus
		{
			Success,
			Failure,
			WrongCode,
			Expired
		}

		public enum UserLoginStatus
		{
			Success,
			Failure,
			Locked,
			NotActivated
		}

		public enum UserRegistrationStatus
		{
			Success,
			Failure,
			AlreadyExists
		}

		public enum BasicStatus
		{
			Success=1,
			Failure=0,
            LimitReached=2
		}

	    public enum UserType
	    {
            RegularUser = 0,
            User = 1
	    }

        public enum LikeAction
        {
            Dislike=0,
            Like=1
        }
	}
}
