namespace Core.EventStore.Test.DockerFramework
{
    public class ImageSettings
    {
        public string Registry { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; } = "latest";

        public string TagQualifiedName => Name + ":" + Tag;
        public string RegistryQualifiedName => string.IsNullOrWhiteSpace(Registry) ? Name : Registry + "/" + Name;
        public string FullyQualifiedName => string.IsNullOrWhiteSpace(Registry) ? TagQualifiedName : Registry + "/" + TagQualifiedName;
    }
}