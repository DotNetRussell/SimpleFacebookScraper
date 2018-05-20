//these are similar to C# using statements
open canopy.runner.classic
open canopy.configuration
open canopy.classic
open System.IO
open System
open OpenQA.Selenium
open System.Collections.Generic

//start an instance of chrome
start chrome

type profileInfo = {
        name : string
        url : string
        education : List<string>
        locations : List<string>
    }

"Facebook Scraper 2.0" &&& fun _ ->

    let target = "John Smith"

    url (sprintf "https://www.facebook.com/public/%s" target)

    let pages = unreliableElements "._262a"

    let pageList = List<int>()
    let pageUrls = List<string>()
    let profileUrls = List<string>()
    let profiles = List<profileInfo>()

    pages |> List.iter(fun page ->
                                    let pageText = page.Text
                                    
                                    if page.Text.Contains("Next") |> not then
                                        
                                        let pageNumber = Int32.Parse(pageText)
                                        let linkUrl = page.GetAttribute("href")
                                        pageList.Add(pageNumber)
                                        pageUrls.Add(linkUrl)

                                        Console.WriteLine (sprintf "Page Number %i found!" pageNumber)
                                        Console.WriteLine (sprintf "Link: %s" linkUrl)
                                    
                                    )


    let profileElements = unreliableElements "._32mo"

    profileElements |> List.iter(fun profileElement ->
                                                        let url = profileElement.GetAttribute("href")
                                                        Console.WriteLine (sprintf "Found Profile Link: %s" url)
                                                        profileUrls.Add(url)
                                                    )
    
    pageUrls |> Seq.iter (fun link ->
                                        url link
                                        
                                        let profileElements = unreliableElements "._32mo"

                                        profileElements |> List.iter(fun profileElement ->
                                                                        let url = profileElement.GetAttribute("href")
                                                                        Console.WriteLine (sprintf "Found Profile Link: %s" url)
                                                                        profileUrls.Add(url) )

                                    )

    profileUrls |> Seq.iter(fun link ->
                                        url link
                                        let profile = {
                                            name =
                                                let coverName = element "#fb-timeline-cover-name"
                                                coverName.Text
                                            url = link
                                            education = 
                                                let schools = unreliableElements "._2lzr"
                                                let schoolList = List<string>()
                                                schools |> List.iter(fun school -> schoolList.Add(school.Text))
                                                schoolList
                                            locations = 
                                                let locations = unreliableElements "._2iel"
                                                let locationList = List<string>()
                                                locations |> List.iter(fun location -> locationList.Add(location.Text))
                                                locationList
                                        }
                                        profiles.Add(profile)
                                        Console.WriteLine (sprintf "Name: %s\r\nURL: %s\r\nSchools: %s\r\nLocations: %s" profile.name profile.url (String.Join(",", profile.education.ToArray())) (String.Join(",", profile.locations.ToArray() ) )) 
                                    )

        
run()

printfn "press [enter] to exit"
System.Console.ReadLine() |> ignore

quit()