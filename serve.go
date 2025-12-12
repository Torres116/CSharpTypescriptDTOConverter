package main

import (
    "fmt"
    "os"
    "os/exec"
)

const publishDir = "./bin"
const wwwrootDir = "./bin/wwwroot"
const basePath = "/CSharpTypescriptDTOConverter"

func runCommand(name string,args ...string) error {
    cmd := exec.Command(name,args...)
    cmd.Stdout = os.Stdout
    cmd.Stderr = os.Stderr
    return cmd.Run()
}

func main(){

    // Delete folder if it exists first
    if _, err := os.Stat(publishDir); err == nil {
        fmt.Println("Deleting existing publish folder...")
        if err := os.RemoveAll(publishDir); err != nil {
            fmt.Println("Failed to delete publish folder:", err)
            return
        }
    }

    // Publish the app
    fmt.Println("Publishing Blazor standalone app...")
    if err := runCommand("dotnet","publish","-c","Release","-o",publishDir); err != nil {
        fmt.Println("Publish failed: ", err)
        return
    }

    // Serve the app
    fmt.Println("Serving Blazor app...")
    if err := runCommand("dotnet", "serve","-d", wwwrootDir,"--path-base",basePath,"-p","59973"); err != nil {
        fmt.Println("Serve failed:", err)
        return
    }
}