<#
.SYNOPSIS
This script is designed to clean specified directories and files in a project.
It provides a flexible way to remove build artifacts, temporary files, and other not needed directories and files.

.DESCRIPTION
This script offers a way to systematically remove unwanted files and directories from a project, improving cleanliness and maintainability.

.AUTHOR
Nikita Neverov (BMTLab)

.VERSION
1.1.0

.EXTERNALHELP
https://gist.github.com/BMTLab/802b9913a5edc82d2988e762d4e2320e
#>
function Clean-Directories
{
    param (
        [Parameter(Mandatory = $true)]
        [string]$Pattern
    )

    Write-Host "Cleaning directories matching pattern: $Pattern"
    Get-ChildItem -Path .. -Recurse -Directory -Filter $Pattern | ForEach-Object {
        Write-Host "-- Removing directory: $( $_.FullName )"
        Remove-Item $_.FullName -Recurse -Force
    }
}

# Function to clean files matching a specific pattern.
function Clean-Files
{
    param (
        [Parameter(Mandatory = $true)]
        [string]$Pattern
    )

    Write-Host "Cleaning files matching pattern: $Pattern"
    Get-ChildItem -Path .. -Recurse -File -Filter $Pattern | ForEach-Object {
        Write-Host "-- Removing file: $( $_.FullName )"
        Remove-Item $_.FullName -Force
    }
}

# Main function to orchestrate cleaning operations.
function Main
{
    Write-Host "Starting full cleaning process..."

    # List of patterns for directories to clean
    $dirPatterns = @('build*', 'bin', 'obj', 'artifacts', 'out', 'StrykerOutput')

    # Clean directories
    foreach ($pattern in $dirPatterns)
    {
        Clean-Directories -Pattern $pattern
    }

    $filesPatterns = @('VERSION.g.txt', 'packages.lock.json',  '.AssemblyAttributes')

    # Clean files
    foreach ($pattern in $filesPatterns)
    {
        Clean-Files -Pattern $pattern
    }

    Write-Host "Cleaning completed!"
}

# Execute the main function
Main