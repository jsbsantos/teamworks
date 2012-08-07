namespace Teamworks.Core.Services.RavenDb.Indexes
{
    /*
    public class Timelog_Filter : AbstractIndexCreationTask<Activity, Activity>
    {
        public class Projection
        {
            public string Person { get; set; }
            public Timelog Timelog { get; set; }
            
            public string Project { get; set; }
            public string Activity { get; set; }
        }

        public Timelog_Filter()
        {
            Map = activities => from act in activities
                                from timelog in act.Timelogs
                                select new
                                           {
                                               Person = timelog.Person,
                                               Project = act.Project,
                                               Activity = act.Id,
                                           };
            TransformResults = (database, activities) =>
                               from act in activities
                               from timelog in act.Timelogs
                               let person = database.Load<Person>(timelog.Person)
                               let project = database.Load<Project>(act.Project)
                               select new
                                          {
                                              Person = person.Name,
                                              Project = project.Name,
                                              Activity = act.Name,
                                              Timelog = timelog
                                          };
        }
    }*/
}