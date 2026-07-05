using System.Security.Cryptography.X509Certificates;
using FindYOU;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class LikeController : Controller
    {
      private  int? UserId ;
      private readonly ILikeInterface _likeRepo;
      public LikeController(ILikeInterface likeRepo)
      {
        _likeRepo = likeRepo;
      }
      
       
        [HttpPost]
public async Task<IActionResult> AddLike(int chatId)
{


    if(chatId == null)
            {
                return BadRequest(new
                {
                    message = "error canot find chat id "
                });
            }
    int result = CheckAuth();

    if (result == 0)
    {
        return RedirectToAction("Login" , "Home");
    }


int userid = (int)UserId;

bool isLiked = await _likeRepo.HasUserLikedChatAsync(chatId , userid);

System.Console.WriteLine(isLiked);

if(isLiked)
            {
                return BadRequest(new
                {
                    success  = false ,
                    message = "You have alredy like it "
                });
            }
Like like = new Like
{
    ChatEntryId = chatId,
UserId = userid
};



            try
            {
               await _likeRepo.AddLikeAsync(like);
               await _likeRepo.Save();
               return Ok(new
               {
                   success = true,
                   message = "Liked successfully"
               });
       
                        }catch(Exception e)
            {
               
                System.Console.WriteLine("there is an error while adding likr");
               return BadRequest();
            }




    
}


[HttpPost]
public async Task<IActionResult> DeleteLike(int chatId)
        {
            if(chatId == null)
            {
                return BadRequest(new
                {
                    message = "error canot find chat id "
                });
            }
    int result = CheckAuth();

    if (result == 0)
    {
        return RedirectToAction("Login" , "Home");
    }


int userid = (int)UserId;

            try
            {
                await _likeRepo.DeleteLikeAsync(chatId , userid);
                 await _likeRepo.Save();
                return Ok(new
                {
                    success = true ,
                    message = "deleted"
                });

            }catch(Exception e)
            {
                System.Console.WriteLine("there is an error while deleting");
                return BadRequest();
            }
        }


public async Task<IActionResult> LikeCountByChat(int chatId)
        {
            if(chatId == null)
            {
                return BadRequest(new
                {
                    message = "error canot find chat id "
                });
            }
    int result = CheckAuth();

    if (result == 0)
    {
        return RedirectToAction("Login" , "Home");
    }


int userid = (int)UserId;

int count = await _likeRepo.GetLikeCountByChatAsync(chatId);

return Ok(count);
        }

public int CheckAuth()
        {
            
            int? result = HttpContext.Session.GetInt32("Userid");
            string? role = HttpContext.Session.GetString("Role");


            if(result.HasValue && role == "User")
            {
                UserId = result;
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
