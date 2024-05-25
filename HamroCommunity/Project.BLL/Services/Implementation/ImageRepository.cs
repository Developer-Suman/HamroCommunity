using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Project.BLL.Services.Interface;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Implementation
{
    public class ImageRepository : IimageRepository
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageRepository(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _contextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            
        }
        public async Task<List<string>> AddMultiple(List<IFormFile> File)
        {
            try
            {
                List<string> filename = new List<string>();
                string uploadFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                string uploadFolderPathForFiles = Path.Combine(_webHostEnvironment.WebRootPath, "Files");

                if(!Directory.Exists(uploadFolderPathForFiles) || !Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPathForFiles);
                    Directory.CreateDirectory(uploadFolderPath);
                }


                foreach(var image in File)
                {
                    string uniqueFile = Guid.NewGuid().ToString();
                    string originalFileName = Path.GetFileName(image.FileName);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
                    string fileExtension = Path.GetExtension(image.FileName);

                    //Combine uploadFolderPath with uniquefile and fileExtension
                    string filePath = Path.Combine(uploadFolderPath, fileNameWithoutExtension + '~' + fileExtension);

                    //Copy image to the server
                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    filename.Add(Path.Combine("Images/", fileNameWithoutExtension + '~' + uniqueFile + fileExtension));
                }

                return filename;
              

            }catch(Exception ex)
            {
                throw new Exception("An error occured while Adding multiple images");
            }
        }

        public async Task<string> AddSingle(IFormFile File)
        {
            try
            {
                string uploadFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                string uploadFolderPathForFiles = Path.Combine(_webHostEnvironment.WebRootPath, "Files");
                if(!Directory.Exists(uploadFolderPathForFiles) || !Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                    Directory.CreateDirectory(uploadFolderPathForFiles);
                }


                string uniqueFile = Guid.NewGuid().ToString();
                string originalFileName = Path.GetFileName(File.FileName);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(File.FileName);
                string fileExtension = Path.GetExtension(originalFileName);


                //combine the uploadfolder path with the uniquefile name
                string filePath = Path.Combine(uploadFolderPath, fileNameWithoutExtension+'~'+uniqueFile+fileExtension);

                //copy file to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await File.CopyToAsync(fileStream);
                }


                return Path.Combine("Images/", fileNameWithoutExtension + '~' + originalFileName + fileExtension);

            }catch(Exception ex)
            {
                throw new Exception("An error occured while Adding Images");
            }
        }

        public void DeleteMultiple(List<string> ImageUrls)
        {
            try
            {
                foreach(var imageUrl in  ImageUrls)
                {
                    var webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl);
                    if(File.Exists(webRootPath))
                    {
                        File.Delete(webRootPath);
                    }    

                }

            }catch (Exception ex)
            {
                throw new Exception("An error occured while deleting image");
            }
        }

        public void DeleteSingle(string ImageUrl)
        {
            try
            {
                var webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, ImageUrl);
                if(File.Exists(webRootPath))
                {
                    File.Delete(webRootPath);
                }
               

            }catch(Exception ex)
            {
                throw new Exception("An error occured while deleting images");
            }
        }

        public async Task<List<string>> UpdateMultiple(List<IFormFile> file, List<string> ImageUrls)
        {
            try
            {
                List<string> multipleImageUrls = new List<string>();
                List<string> deleteImageFiles = new List<string>();

                foreach(var uploadedFile in file)
                {
                    //Get the filesname from the uploaded file
                    var fileNameFromUploadedFile = Path.GetFileName(uploadedFile.FileName);
                    string fileNameFromUploadedFileWithoutExtension = Path.GetFileNameWithoutExtension(uploadedFile.FileName);

                    //Check if the filename matches any existing image
                    bool foundMatch  = false;

                    foreach(var existingImageUrl in ImageUrls)
                    {
                        var webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, existingImageUrl);
                        var fileName = Path.GetFileName(webRootPath);

                        // Get the filename from the path
                        var filename = Path.GetFileName(webRootPath);
                        var imageName = filename.Split('~');


                        // If a match is found, add the existing URL and skip to the next file
                        if (imageName[0] == fileNameFromUploadedFileWithoutExtension)
                        {
                            multipleImageUrls.Add(existingImageUrl);
                            foundMatch = true;
                            break;
                        }

                    }

                    // If no match is found, upload the new image
                    if (!foundMatch)
                    {
                        var updateImage = await AddSingle(uploadedFile);
                        multipleImageUrls.Add(updateImage);
                    }
                }

                //Now we have the updated list of image urls
                //Lets Find the images to delete (if any)
                foreach(var existingImageURL in ImageUrls)
                {
                    //If the existing image URLs is not in the list of Updated URL, add it to delete
                    if(!multipleImageUrls.Contains(existingImageURL))
                    {
                        deleteImageFiles.Add(existingImageURL);
                    }
                }

                //Delete the image that are no longer in the updated list
                if(deleteImageFiles.Count() > 0)
                {
                    DeleteMultiple(deleteImageFiles);
                }

                //Return the updated list of image URLs
                return multipleImageUrls;
                
            }catch(Exception ex)
            {
                throw new Exception("An error occured while updating image");
            }
        }

        public async Task<string> UpdateSingle(IFormFile file, string ImageUrl)
        {
            try
            {
                if(file is null || file.Length == 0)
                {
                    return ImageUrl;
                }
                DeleteSingle(ImageUrl);
                var saveImage = await AddSingle(file);
                return saveImage;

            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while updating Images");
            }
        }
    }
}
