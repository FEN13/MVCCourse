using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using Microsoft.Web.Mvc;
using Newtonsoft.Json.Serialization;
using PhotoGalery.Data;
using PhotoGalery.Data.Migrations;
using PhotoGalery.DTOs;

namespace PhotoGalery
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			//Clear view engines
			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new FixedRazorViewEngine());

			//Remove XML formatter
			GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

			//Setup proper json formatting
			GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			AreaRegistration.RegisterAllAreas();

			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			BundleTable.EnableOptimizations = false;
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhotoGaleryDataContext, Configuration>());

			#region Mapper configuration

			Mapper.CreateMap<Data.Models.User, UserListItem>()
				.ForMember(d => d.Role, o => o.MapFrom(s => s.Role.Name))
				.ForMember(d => d.LastActiveDate, o => o.MapFrom(s => s.LastActiveDate.ToShortDateString()))
                .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt.ToShortDateString()))
                .ForMember(d => d.PhotoLimit, o => o.MapFrom(s => -1))
                .ForMember(d => d.AlbumLimit, o => o.MapFrom(s => -1));

            Mapper.CreateMap<Data.Models.RegularUser, UserListItem>()
                .ForMember(d => d.Role, o => o.MapFrom(s => s.Role.Name))
                .ForMember(d => d.LastActiveDate, o => o.MapFrom(s => s.LastActiveDate.ToShortDateString()))
                .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt.ToShortDateString()));

		    Mapper.CreateMap<Data.Models.Device, Device>();
            Mapper.CreateMap<Data.Models.Model, Model>()
                .ForMember(d => d.Manufacturer, o => o.MapFrom(s => s.Manufacturer.Id));
            Mapper.CreateMap<Data.Models.Manufacturer, Manufacturer>();
			
			Mapper.CreateMap<Data.Models.RegularUser, UserListItem>()
				.ForMember(d => d.Role, o => o.MapFrom(s => s.Role.Name))
				.ForMember(d => d.LastActiveDate, o => o.MapFrom(s => s.LastActiveDate.ToShortDateString()))
				.ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt.ToShortDateString()));

		    Mapper.CreateMap<Data.Models.Album, Album>()
		        .ForMember(d => d.NewName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Cover, o => o.MapFrom(s => s.CoverId != 0 ? s.Photos.FirstOrDefault(c => c.Id == s.CoverId) != null ? s.Photos.FirstOrDefault(c => c.Id == s.CoverId).Thumb : new byte[0] : s.Photos.FirstOrDefault() != null ? s.Photos.FirstOrDefault().Thumb : new byte[0]));

			Mapper.CreateMap<Data.Models.Role, Role>();

		    Mapper.CreateMap<Data.Models.Photo, Photo>()
		        .ForMember(d => d.DeviceManufacturer, o => o.MapFrom(s => s.Device.Manufacturer.Name))
		        .ForMember(d => d.DeviceModel, o => o.MapFrom(s => s.Device.Model.Name))
                .ForMember(d => d.IsCover, o => o.MapFrom(s => false))
                .ForMember(d => d.AlbumName, o => o.MapFrom(s => s.Album.Name));

			Mapper.AssertConfigurationIsValid();

			#endregion
		}
	}
}