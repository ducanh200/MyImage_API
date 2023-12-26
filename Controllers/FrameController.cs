﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyImage_API.DTOs;
using MyImage_API.Entities;
using MyImage_API.Models;
using MyImage_API.Models.Frame;

namespace MyImage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrameController : ControllerBase
    {
        private readonly MyimageContext _context;
        private readonly IWebHostEnvironment _environment;
        public FrameController(MyimageContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        [HttpGet]
        public IActionResult Index()
        {
            List<Frame> frames = _context.Frames.ToList();
            if (frames.Count == 0)
            {
                return BadRequest("Không có khung trong cơ sở dữ liệu!");
            }
            List<FrameDTO> data= new List <FrameDTO>();
            foreach (Frame n in frames)
            {
                data.Add(new FrameDTO
                {
                    id = n.Id,
                    frame_amount = n.FrameAmount,
                    frame_color = n.FrameColor,
                    frame_name  = n.FrameName,
                });
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id) 
        {
            try
            {
                Frame f = _context.Frames.Where(f => f.Id == id).First();
                if (f == null)
                    return NotFound();
                return Ok(new FrameDTO
                {
                    id=f.Id,
                    frame_amount = f.FrameAmount,
                    frame_color = f.FrameColor,
                    frame_name = f.FrameName,
                });

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create(CreateFrame model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    Frame data = new Frame {FrameAmount = model.frame_amount,FrameColor = model.frame_color, FrameName = model.frame_name};
                    _context.Frames.Add(data);
                    _context.SaveChanges();
                    return Created($"get-by-id?id={data.Id}",
                    new FrameDTO { id = data.Id,
                        frame_amount = data.FrameAmount,
                        frame_color = data.FrameColor,
                        frame_name = data.FrameName 
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            var msgs = ModelState.Values.SelectMany(v => v.Errors)
                .Select(v => v.ErrorMessage);
            return BadRequest(string.Join(" | ", msgs));
        }

        [HttpPut]
        public IActionResult Edit(EditFrame model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    Frame frame = new Frame { Id = model.id,
                        FrameAmount = model.frame_amount,
                        FrameColor = model.frame_color,
                        FrameName = model.frame_name
                    };
                    if (frame != null) 
                    { 
                        _context.Frames.Update(frame);
                        _context.SaveChanges();
                        return Ok("Thay đổi thông tin khung thành công !");
                    }

                }catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }
    }
}