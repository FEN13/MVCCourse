// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using PhotoGalery.Data;
using PhotoGalery.Mailer;
using PhotoGalery.Services;
using PhotoGalery.Services.Interfaces;
using StructureMap;
namespace PhotoGalery.DependencyResolution {
    public static class IoC {
        public static IContainer Initialize() {
			ObjectFactory.Initialize(x =>
			{
				x.Scan(scan =>
				{
					scan.TheCallingAssembly();
					scan.WithDefaultConventions();
				});
				x.For<IMailer>().Use<Mailer.Mailer>();

				x.For<IPhotoGaleryContextFactory>().Use<PhotoGaleryContextFactory>();
				x.For<IAuthService>().Use<AuthService>();
				x.For<IUsersService>().Use<UsersService>();
				x.For<IAlbumsServices>().Use<AlbumsServices>();
				x.For<IPhotoService>().Use<PhotoService>();
			});
            return ObjectFactory.Container;
        }
    }
}