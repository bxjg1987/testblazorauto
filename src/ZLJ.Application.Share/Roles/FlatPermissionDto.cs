namespace ZLJ.Application.Share.Roles
{
    public record FlatPermissionDto
    {
        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public int Level { get; set; }


        public IEnumerable<FlatPermissionDto> Children { get; set; }

        //public bool IsGrantedByDefault { get; set; }
    }
}