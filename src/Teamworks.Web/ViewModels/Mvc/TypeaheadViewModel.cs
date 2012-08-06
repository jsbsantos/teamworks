namespace Teamworks.Web.ViewModels.Mvc
{
    public class TypeaheadViewModel
    {
        public Entity Activity { get; set; }
        public Entity Project { get; set; }

        #region Nested type: Entity

        public class Entity
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        #endregion
    }
}