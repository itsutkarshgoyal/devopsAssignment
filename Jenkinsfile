pipeline{
    agent:any
    environment{
      scannerHome = tool name:'SonarQubeScanner'
      username = 'utkarshgoyal'

    }
    
    stages
    {
        stage("Start"){
            steps{
               checkout scm
            }
        }

        stage("Nuget Restore"){
            steps{
                echo "Nuget Retore"
                bat "dotnet nuget restore"
            }
        }

       stage("Start SonarQube"){
           steps{
               withSonarQubeEnv("Test_sonar"){
                   bat "${scannerHome}/SonarScanner.MSBuild.exe begin /k: SampleWebApp/ /d:sonar.cs.opencober.reportsPaths=Coverge.opencover.xml"

               }
           }
       }
        stage("Build"){
            steps{
                echo "build"
                bat "dotnet build -c Release -o SampleWebApp/app/build/SampleWebApp.csproj"
            }
        }

        stage("Start Testing"){
            steps{
                echo "begin testing"
                bat "dotnet test SampleApplicationTest\\SampleApplicationTest.csproj /p:CollectCoverage=true /:p CoverletOutputFormat=openCover"
            }
        }

         stage("Stop SonarQube"){
           steps{
               withSonarQubeEnv("Test_sonar"){
                   bat "${scannerHome}/SonarScanner.MSBuild.exe end"

               }
           }
       }

       stage("Docker Image"){
           steps{
               echo "Docker image creation"
               bat 'dotnet publish -c Release'
               bat 'docker build -t {username} --no-cache -f Dockerfile .'
           }
       }

       stage("Pre Container check"){
           steps{
               script{
                   String dockerCommand = "docker ps -a -q -f name=${container_name}"
                   String command = "${bat( return StdOut:true, script:dockerCommand)}"
                   String previous_id= "${command.trim().readLines().drop(1).join('')}"
                   if(previous_id !=""){
                       echo "previous id found ${previous_id}"
                       echo "stop container"
                       bat "docker stop ${previous_id}"
                       bat "docker rm ${previous_id}"
                   }else {
                       echo "previous id not found"
                   }
               }
           }
       }
    }
}