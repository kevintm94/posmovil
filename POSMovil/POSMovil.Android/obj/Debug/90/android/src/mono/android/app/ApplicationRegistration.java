package mono.android.app;

public class ApplicationRegistration {

	public static void registerApplications ()
	{
				// Application and Instrumentation ACWs must be registered first.
		mono.android.Runtime.register ("POSMovil.Droid.MainApplication, POSMovil.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", crc640c4bf90639d5ad10.MainApplication.class, crc640c4bf90639d5ad10.MainApplication.__md_methods);
		mono.android.Runtime.register ("Shiny.ShinyAndroidApplication, Shiny.Core, Version=1.1.0.1473, Culture=neutral, PublicKeyToken=null", crc643387a08acbe69b14.ShinyAndroidApplication.class, crc643387a08acbe69b14.ShinyAndroidApplication.__md_methods);
		mono.android.Runtime.register ("Shiny.ShinyAndroidApplication`1, Shiny.Core, Version=1.1.0.1473, Culture=neutral, PublicKeyToken=null", crc643387a08acbe69b14.ShinyAndroidApplication_1.class, crc643387a08acbe69b14.ShinyAndroidApplication_1.__md_methods);
		
	}
}
