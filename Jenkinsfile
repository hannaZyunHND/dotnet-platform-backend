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
                git branch: 'main', url: 'https://github.com/hannaZyunHND/dotnet-platform-backend.git',
                credentialsId: 'Jenkins_PAT'
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
                    sh 'docker build -f Web/EvisaV2/CMS/EvisaV2CMS/Dockerfile --force-rm -t evisa-v2-cms .'
                }
            }
        }
        stage('Push Docker Image') {
            steps {
                withDockerRegistry(credentialsId: 'docker-hub', url: 'https://index.docker.io/v1/') {
                    sh 'docker tag evisa-v2-cms phamdat2002/evisa-v2-cms:latest'
                    sh 'docker push phamdat2002/evisa-v2-cms:latest'
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