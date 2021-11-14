using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GomokuGame;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GomokuGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GomokuGameController : ControllerBase
    {
        readonly GomokuGame.GomokuGame Game;

        public GomokuGameController(GomokuGame.GomokuGame game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }
        

        [HttpGet("status")]
        public GameStatusDto GetStatus()
        {
            return new GameStatusDto(Game);
        }


        [HttpPost("place-stone")]
        public ActionResult<GameStatusDto> PlaceStone([FromBody] CellCoordinates coords)
        {
            try
            {
                Game.PlaceStone(coords);
                return CreatedAtAction(nameof(PlaceStone), new GameStatusDto(Game));
            }
            catch (GomokuGameException gex)
            {
                return UnprocessableEntity(gex.Message);
            }
        }

        [HttpPost("restart")]
        public ActionResult<GameStatusDto> Restart()
        {
            Game.Restart();
            return CreatedAtAction(nameof(Restart), new GameStatusDto(Game));
        }
    }
}
