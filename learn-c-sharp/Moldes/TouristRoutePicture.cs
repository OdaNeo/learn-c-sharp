﻿namespace learn_c_sharp.Moldes
{
    public class TouristRoutePicture
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public Guid TouristRouteId { get; set; }
        public TouristRoute TouristRoute { get; set; }
    }
}
