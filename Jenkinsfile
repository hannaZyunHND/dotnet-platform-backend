pipeline {
    agent any
    environment {
        DOCKER_CREDENTIALS_ID = 'docker-hub-login'
    }
    stages {
        stage('Debug Docker') {
            steps {
                sh 'docker --version'
            }
        }

        stage('Clone Repository') {
            steps {
                git branch: 'platform_webapi_dev', url: 'https://github.com/hannaZyunHND/dotnet-platform-backend.git',
                credentialsId: 'jenkin-huy-access'
            }
        }

        stage('Set Permissions') {
            steps {
                sh 'chmod +x deploy.sh'
                sh 'sudo usermod -aG docker jenkins'
            }
        }
        
        stage('Build Docker Image') {
            steps {
                withDockerRegistry(credentialsId: 'docker-hub', url: 'https://index.docker.io/v1/') {
                    sh 'docker build --build-arg ENVIRONMENT=development -f Web/Platform/WEBAPI/PlatformWEBAPI/Dockerfile --force-rm -t platform_webapi_dev .'
                }
            }
        }
        stage('Push Docker Image') {
            steps {
                withDockerRegistry(credentialsId: 'docker-hub', url: 'https://index.docker.io/v1/') {
                    sh 'docker tag platform_webapi_dev phamdat2002/platform_webapi_dev:latest'
                    sh 'docker push phamdat2002/platform_webapi_dev:latest'
                }
            }
        }
        stage('Deploy Docker Image') {
            steps {
                script {
                    sh './deploy.sh'
                }
            }
        }
    }
}

