﻿using System.Linq;
using System.Net.Http;
using Raven.Client.Document;
using Teamworks.Core.Services;
using Teamworks.Web.Models.Api;
using Teamworks.Web.Test.Api.Fixture;
using Xunit;
using Teamworks.Web.Controllers.Api;

namespace Teamworks.Web.Test.Api
{
    public class ProjectsControllerTest : IUseFixture<DocumentStoreFixture>
    {
        public DocumentStoreFixture Fixture { get; set; }

        [Fact]
        public void GetProjects()
        {
            var controller = new ProjectsController(Global.Store.OpenSession());
            var size = controller.Get().Count();
            Fixture.Store(Core.Project.Forge("proj 1", "proj 1 description"));

            Assert.Equal(size, controller.Get().Count());
        }

        [Fact]
        public void GetProjectById()
        {
            var name = "proj 1";
            var description = "description 1";
            var controller = new ProjectsController();
            var project = Core.Project.Forge(name, description);
            Fixture.Store(project);

            var result = controller.Get(project.Identifier);
            Assert.NotNull(result);
            Assert.Equal(project.Name, result.Name);
            Assert.Equal(project.Description, result.Description);
        }

        [Fact]
        public void PostProject()
        {
            /*
            var name = "proj 1";
            var description = "description 1";

            var controller = new ProjectsController();
            var project = controller.Post(new Project
                                {
                                    Name = name,
                                    Description = description
                                }).Content.ReadAsAsync<Project>().Result;

            var result = Fixture.Load<Core.Project>(project.Id);
            Assert.NotNull(result);
            Assert.Equal(project.Name, result.Name);
            Assert.Equal(project.Description, result.Description);
            Assert.False(result.Archived);
            */
        }

        public void SetFixture(DocumentStoreFixture data)
        {
            Fixture = data;
            Fixture.Initialize();
        }
    }
}