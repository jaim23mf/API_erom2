using euroma2.Models;
using euroma2.Models.Blog_model;
using euroma2.Models.Events;
using euroma2.Models.Firebase;
using euroma2.Models.Promo;
using euroma2.Services;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace euroma2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class FirebaseController : Controller
    {
        private readonly DataContext _dbContext;
        private readonly PtaInfo _options;
        private FirebaseApp fa;
        public FirebaseController(DataContext dbContext, IOptions<PtaInfo> options)
        {
            _dbContext = dbContext;
            this._options = options.Value;

            if (FirebaseApp.DefaultInstance == null)
            {
                fa = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("./token.json"),
                    ProjectId = "pta-wayfinding",
                });
            }
            else {
                fa = FirebaseApp.DefaultInstance;
            }

    }

        [HttpGet("Firebase")]
        public async Task<ActionResult<IEnumerable<Firebase_model>>> GetFirebase()
        {
            if (_dbContext.fire == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .fire
                .ToListAsync();

            if (t == null)
            {
                return NotFound();
            }

            return t;

        }


        [HttpGet("Firebase/Type/{id}")]
        public async Task<ActionResult<IEnumerable<NotificationIds>>> GetFirebaseType(int id)
        {

            var t = new List<NotificationIds>();

            switch (id) { 
                case 0: //Promotion

                    if (_dbContext.promotion == null)
                    {
                        return NotFound();
                    }
                    var l  = await _dbContext
                        .promotion
                        .ToListAsync();
                    t = cast(l);
                    break;
                case 1: //Shop
                    if (_dbContext.shop == null)
                    {
                        return NotFound();
                    }
                    var l1 = await _dbContext
                        .shop
                        .ToListAsync();
                    t = cast2(l1);

                    break;
                case 2: //Events
                    if (_dbContext.events == null)
                    {
                        return NotFound();
                    }
                    var l2 = await _dbContext
                        .events
                        .ToListAsync();
                    t = cast1(l2);
                    break;
                default:
                    return t; 
            }

            if (t == null)
            {
                return NotFound();
            }

            return t;

        }


        private List<NotificationIds> cast(List<Promotion> p) { 
            List<NotificationIds> t = new List<NotificationIds>();
            foreach (var promotion in p)
            {
                NotificationIds n = new NotificationIds();
                n.id = promotion.id;
                n.title = promotion.title;
                t.Add(n);
            }
            return t;
        }
        private List<NotificationIds> cast1(List<Mall_Event> p)
        {
            List<NotificationIds> t = new List<NotificationIds>();
            foreach (var ev in p)
            {
                NotificationIds n = new NotificationIds();
                n.id = ev.id;
                n.title = ev.title;
                t.Add(n);
            }
            return t;
        }
        private List<NotificationIds> cast2(List<Shop> p)
        {
            List<NotificationIds> t = new List<NotificationIds>();
            foreach (var shop in p)
            {
                NotificationIds n = new NotificationIds();
                n.id = shop.id;
                n.title = shop.title;
                t.Add(n);
            }
            return t;
        }
        [HttpPost("Firebase")]
        [Authorize]
        public async Task<ActionResult<Firebase_model>> PostFirebase(Firebase_model fb)
        {
            /*_dbContext.reach.Add(reach);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetReach), new { id = reach.id }, reach);*/

            Firebase_model p = new Firebase_model();
            p.title = fb.title;
            p.date = fb.date;
            p.image = fb.image;
            p.msg = fb.msg;
            p.name = fb.name;
            p.notificationId = fb.notificationId;
            p.notificationType = fb.notificationType;
            p.notificationName = fb.notificationName;
            p.target = fb.target;

            _dbContext.fire.Add(p);

            await _dbContext.SaveChangesAsync();
            var res = CreatedAtAction(nameof(GetFirebase), new { id = p.id, lang = "en" }, p);


            await sendMsgAsync(p);

            return res;
        }


        private async Task sendMsgAsync(Firebase_model p) {

            /*if (FirebaseMessaging.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    //Credential = GoogleCredential.GetApplicationDefault(),
                    Credential = GoogleCredential.FromFile("./token.json"),
                    ProjectId = "pta-wayfinding",
                });
            }*/

            var msg = FirebaseMessaging.GetMessaging(fa);


            var topic = p.getTarget(p.target);

            // See documentation on defining a message payload.


            var message = new Message()
            {
                Android = new AndroidConfig {
                Notification = new AndroidNotification { 
                    Title = p.title,
                    Body = p.msg
                }
                },
                Notification = new Notification {
                    Title = p.title,
                    Body = p.msg
                },
                Apns = new ApnsConfig { 
                    Aps = new Aps() {
                        CustomData = new Dictionary<string, object>() {
                            {"notificationType",p.notificationType },
                            {"notificationId", p.notificationId }
                        },
                    }
                },

                Topic = topic,
            };

            // Send a message to the devices subscribed to the provided topic.
            string response = await msg.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);
        }


        [HttpPost("Firebase/ImgUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles(this._options);
            uf = await uf.UploadFileToAsync(Consts.FireImg, file);
            return Ok(uf);
        }




    }
}
