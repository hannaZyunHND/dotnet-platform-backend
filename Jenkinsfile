pipeline {
    agent any
    environment {
        DOCKER_CREDENTIALS_ID = 'docker-hub'
    }
    stages {
        stage('Debug Docker') {
            steps {
                sh 'docker --version'
            }
        }
        stage('Clone Repository') {
            steps {
                git branch: 'publish', url: 'https://github.com/hannaZyunHND/dotnet-platform-backend',
                credentialsId: 'jenkin-huy-access'
            }
        }
        stage('Set Permissions') {
            steps {
                sh 'chmod +x deploy.sh'
                sh 'sudo usermod -aG docker jenkins'
            }
        }
        stage('Build Docker Images') {
            parallel {
                stage('Build CMS') {
                    steps {
                        sh 'DOCKER_BUILDKIT=1 docker build -f Web/Platform/CMS/PlatformCMS/Dockerfile --force-rm -t joytime-web-cms .'
                    }
                }
                stage('Build API') {
                    steps {
                        sh 'DOCKER_BUILDKIT=1 docker build -f Web/Platform/WEBAPI/PlatformWEBAPI/Dockerfile --force-rm -t joytime-web-api .'
                    }
                }
            }
        }
        stage('Tag and Push Docker Images') {
            parallel {
                stage('Tag and Push CMS') {
                    steps {
                        withDockerRegistry(credentialsId: 'docker-hub', url: 'https://index.docker.io/v1/') {
                            sh 'docker tag joytime-web-cms consortio/joytime-web-cms:latest'
                            sh 'docker push consortio/joytime-web-cms:latest'
                        }
                    }
                }
                stage('Tag and Push API') {
                    steps {
                        withDockerRegistry(credentialsId: 'docker-hub', url: 'https://index.docker.io/v1/') {
                            sh 'docker tag joytime-web-api consortio/joytime-web-api:latest'
                            sh 'docker push consortio/joytime-web-api:latest'
                        }
                    }
                }
            }
        }
        stage('Deploy Docker Images') {
            steps {
                sh './deploy.sh'
            }
        }
    }
}
