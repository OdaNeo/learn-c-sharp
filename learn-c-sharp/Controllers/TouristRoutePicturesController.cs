﻿using AutoMapper;
using learn_c_sharp.Dtos;
using learn_c_sharp.Models;
using learn_c_sharp.Services;
using Microsoft.AspNetCore.Mvc;

namespace learn_c_sharp.Controllers
{
    [Route("api/touristRoutes/{touristRouteId}/pictures")]
    [ApiController]
    public class TouristRoutePicturesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        public TouristRoutePicturesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetPictureListForTouristRoute(Guid touristRouteId)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("not found");
            }
            var pictureFromRepo = await _touristRouteRepository.GetPicturesByTouristRouteIdAsync(touristRouteId);
            if (pictureFromRepo == null || pictureFromRepo.Count() == 0)
            {
                return NotFound("not found");
            }
            return Ok(_mapper.Map<IEnumerable<TouristRoutePictureDto>>(pictureFromRepo));
        }
        [HttpGet("{pictureId}", Name = "GetPicture")]
        public async Task<IActionResult> GetPicture(Guid touristRouteId, int pictureId)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("not found");
            }
            var pictureFromRepo = await _touristRouteRepository.GetPictureAsync(pictureId);
            if (pictureFromRepo == null)
            {
                return NotFound("not found");
            }
            return Ok(_mapper.Map<TouristRoutePictureDto>(pictureFromRepo));
        }
        [HttpPost]
        public async Task<IActionResult> CreateTouristRoutePicture(
            [FromRoute] Guid touristRouteId,
            [FromBody] TouristRoutePictureFroCreationDto touristRoutePictureFroCreateDto)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("not found");
            }
            var pictureModel = _mapper.Map<TouristRoutePicture>(touristRoutePictureFroCreateDto);
            _touristRouteRepository.AddTouristRoutePicture(touristRouteId, pictureModel);
            await _touristRouteRepository.SaveAsync();
            var pictureToReturn = _mapper.Map<TouristRoutePictureDto>(pictureModel);
            return CreatedAtRoute(
                "GetPicture", new { touristRouteId = pictureModel.TouristRouteId, pictureId = pictureModel.Id }, pictureToReturn);
        }
        [HttpDelete("{pictureId}")]
        public async Task<IActionResult> DeletePicture([FromRoute] Guid touristRouteId, [FromRoute] int pictureId)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("not found");
            }
            var picture = await _touristRouteRepository.GetPictureAsync(pictureId);
            _touristRouteRepository.DeleteTouristRoutePicture(picture);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }
    }
}
