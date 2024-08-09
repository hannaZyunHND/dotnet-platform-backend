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
                git branch: 'platform_webapi_prod', url: 'https://github.com/hannaZyunHND/dotnet-platform-backend.git',
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
                    sh 'docker build --build-arg ENVIRONMENT=production -f Web/Platform/WEBAPI/PlatformWEBAPI/Dockerfile --force-rm -t platform_webapi_prod .'
                }
            }
        }
        stage('Push Docker Image') {
            steps {
                withDockerRegistry(credentialsId: 'docker-hub', url: 'https://index.docker.io/v1/') {
                    sh 'docker tag platform_webapi_prod phamdat2002/platform_webapi_prod:latest'
                    sh 'docker push phamdat2002/platform_webapi_prod:latest'
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

