using Application.Contracts.DTO;
using Application.JobOffer.Commands;
using AutoMapper;
using GrpcPublish;
using System;
using Application.JobOffer.DTO;

namespace TURI.Contractservice.Grpc.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<List<Application.Contracts.DTO.AvailableUnitsResult>, GrpcPublish.AvailableUnitsResult>();
            CreateMap<Offer, CreateOfferCommand>();
            CreateMap<CreateOfferCommand,Offer>();
            CreateMap<UpdateOfferCommand, Offer>();
            CreateMap<Offer,UpdateOfferCommand>();
            CreateMap<Application.JobOffer.Commands.IntegrationData, GrpcPublish.IntegrationData>();
            CreateMap<GrpcPublish.IntegrationData,Application.JobOffer.Commands.IntegrationData>();
            CreateMap<AtsOffer, GrpcPublish.IntegrationData>();
            CreateMap<GrpcPublish.IntegrationData,AtsOffer>();
        }
    }
}
