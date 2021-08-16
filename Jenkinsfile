pipeline{
    agent any
    environment{
      scannerHome = tool name:'SonarQubeScanner'
      username = 'utkarshgoyal'
      registry = 'utkarshgoyal/devopsassignment'
      container_name = 'utkarshgoyal'


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
                bat "dotnet restore"
            }
        }

       stage("Start SonarQube"){
           steps{
               withSonarQubeEnv("Test_sonar"){
                   bat "${scannerHome}/SonarScanner.MSBuild.exe begin /k:SampleWebApp /d:sonar.cs.opencover.reportsPaths=coverge.opencover.xml"

               }
           }
       }
        stage("Build"){
            steps{
                echo "build"
                bat "dotnet clean"
                bat "dotnet build -c Release -o SampleWebApp/app/build"
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
                   String command = "${bat(returnStdout:true, script:dockerCommand)}"
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

       stage("docker push"){
           steps{
           bat "docker tag {username} ${registry}/:latest"
           bat "docker push ${registry}/:latest"
           }
       }

       stage("docker run"){
           steps{
               bat "docker run --name ${container_name} -d p :80"
           }
       }

       stage("k8 deploy"){
           steps{
               bat "gcloud auth activate-service-account --key-file=spheric-gearing-323104-ff86861a0738.json"
               bat "gcloud container cluster get-credentials cluster-1 -zone us-central1-c-- spheric-gearing-323104-ff86861a0738"
               bat "kubectl apply -f deployment.yaml"
           }
       }
    }
}