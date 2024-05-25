using ImageMagick;
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
        private readonly IHelpherMethods _helpherMethods;

        public ImageRepository(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, IHelpherMethods helpherMethods)
        {
            _contextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _helpherMethods = helpherMethods;
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
                    if (_helpherMethods.IsImage(image.ContentType))
                    {

                        string uniqueFile = Guid.NewGuid().ToString();
                        string originalFileName = Path.GetFileName(image.FileName);
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
                        string FileExtension = Path.GetExtension(originalFileName);


                        //Combine uploadFolderPath with unique file and fileExtension 
                        string filepath = Path.Combine(uploadFolderPath, fileNameWithoutExtension + '~' + uniqueFile + FileExtension);

                        //copy images to the server
                        using (var fileStream = new FileStream(filepath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);

                        }

                        using (MagickImage img = new MagickImage(filepath))
                        {
                            if (FileExtension == ".jpg" || FileExtension == ".jpeg")
                            {
                                img.Format = MagickFormat.Jpg;

                            }
                            if (FileExtension == ".png")
                            {
                                img.Format = MagickFormat.Png;
                            }

                            img.Resize(1000, 1000);

                            img.Quality = 60;

                            string uniqueFileAfterCompression = Guid.NewGuid().ToString();
                            string FileNameAfterCompression = Path.GetFileName(image.FileName);
                            string getFileNameWithoutExtension = Path.GetFileNameWithoutExtension(FileNameAfterCompression);
                            string ExtensionAfterCompression = Path.GetExtension(FileNameAfterCompression);

                            string FilePathAfterCompression = Path.Combine(uploadFolderPath, getFileNameWithoutExtension + '~' + uniqueFileAfterCompression + ExtensionAfterCompression);
                            img.Write(FilePathAfterCompression);

                            System.IO.File.Delete(filepath);

                            filename.Add(Path.Combine("Images/", getFileNameWithoutExtension + '~' + uniqueFileAfterCompression + ExtensionAfterCompression));
                        }

                    }
                    else
                    {

                        string uniqueFile = Guid.NewGuid().ToString();
                        string originalFileName = Path.GetFileName(image.FileName);
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
                        string FileExtension = Path.GetExtension(originalFileName);


                        //Combine uploadFolderPath with unique file and fileExtension 
                        string filepath = Path.Combine(uploadFolderPathForFiles, fileNameWithoutExtension + '~' + uniqueFile + FileExtension);
                        using (var fileStream = new FileStream(filepath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        string uniqueFileNameAfterCompression = Guid.NewGuid().ToString();
                        string filenameAfterCompression = Path.GetFileName(image.FileName);
                        string getFileNameWithoutExtensionAfter = Path.GetFileNameWithoutExtension(filenameAfterCompression);
                        string ExtensionAfterCompression = Path.GetExtension(filenameAfterCompression);

                        string filePathAfterCompression = Path.Combine(uploadFolderPathForFiles, getFileNameWithoutExtensionAfter + '~' + uniqueFileNameAfterCompression + ExtensionAfterCompression);
                        _helpherMethods.CompressFile(filepath, filePathAfterCompression);

                        //Remove the original File
                        System.IO.File.Delete(filepath);

                        filename.Add(Path.Combine("Files/", getFileNameWithoutExtensionAfter + '~' + uniqueFileNameAfterCompression + ExtensionAfterCompression));


                    }
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

                if (_helpherMethods.IsImage(File.ContentType))
                {
                    string uniqueFile = Guid.NewGuid().ToString();
                    string originalFileName = Path.GetFileName(File.FileName);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(File.FileName);
                    string fileExtension = Path.GetExtension(originalFileName);


                    //combine the uploadfolder path with the uniquefile name
                    string filePath = Path.Combine(uploadFolderPath, fileNameWithoutExtension + '~' + uniqueFile + fileExtension);

                    //copy file to the server
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await File.CopyToAsync(fileStream);
                    }

                    using (MagickImage image = new MagickImage(filePath))
                    {
                        //Set the desired format like .png,.jpg
                        if (fileExtension == ".jpg" || fileExtension == ".jpeg")
                        {
                            image.Format = MagickFormat.Jpg;

                        }
                        if (fileExtension == ".png")
                        {
                            image.Format = MagickFormat.Png;
                        }

                        //Resize the image if necessary
                        image.Resize(1000, 1000);

                        //Set the compression Quality(0-100)
                        image.Quality = 60; //This is the compression level

                        string uniqueFileAfterCompression = Guid.NewGuid().ToString();
                        string originalfilenameAfterCompression = Path.GetFileName(File.FileName);
                        string getFileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalfilenameAfterCompression);
                        string fileExtensionAfterCompression = Path.GetExtension(originalfilenameAfterCompression);
                        string filepathAfterCompression = Path.Combine(uploadFolderPath, getFileNameWithoutExtension + '~' + uniqueFileAfterCompression + fileExtensionAfterCompression);
                        image.Write(filepathAfterCompression);

                        System.IO.File.Delete(filePath);

                        return Path.Combine("Images/", getFileNameWithoutExtension + '~' + uniqueFileAfterCompression + fileExtensionAfterCompression);
                    }
                }
                else
                {
                    string uniqueFile = Guid.NewGuid().ToString();
                    string originalFileName = Path.GetFileName(File.FileName);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
                    string FileExtension = Path.GetExtension(originalFileName);


                    //Combine uploadFolderPath with unique file and fileExtension 
                    string filepath = Path.Combine(uploadFolderPathForFiles, fileNameWithoutExtension + '~' + uniqueFile + FileExtension);
                    using (var fileStream = new FileStream(filepath, FileMode.Create))
                    {
                        await File.CopyToAsync(fileStream);
                    }

                    string uniqueFileNameAfterCompression = Guid.NewGuid().ToString();
                    string filenameAfterCompression = Path.GetFileName(File.FileName);
                    string getFileNameWithoutExtensionAfter = Path.GetFileNameWithoutExtension(filenameAfterCompression);
                    string ExtensionAfterCompression = Path.GetExtension(filenameAfterCompression);

                    string filePathAfterCompression = Path.Combine(uploadFolderPathForFiles, getFileNameWithoutExtensionAfter + '~' + uniqueFileNameAfterCompression + ExtensionAfterCompression);
                    _helpherMethods.CompressFile(filepath, filePathAfterCompression);

                    //Remove the original File
                    System.IO.File.Delete(filepath);

                    return Path.Combine("Files/", getFileNameWithoutExtensionAfter + '~' + uniqueFileNameAfterCompression + ExtensionAfterCompression);
                }
              

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
