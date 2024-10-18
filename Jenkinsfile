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
                stage('Build Web Docker Image') {
                    steps {
                        withDockerRegistry(credentialsId: 'docker-hub', url: 'https://index.docker.io/v1/') {
                            sh 'DOCKER_BUILDKIT=1 docker build -f Web/Platform/CMS/PlatformCMS/Dockerfile --force-rm -t joytime-web-cms .'
                        }
                    }
                }
                stage('Build CMS Docker Image') {
                    steps {
                        withDockerRegistry(credentialsId: 'docker-hub', url: 'https://index.docker.io/v1/') {
                            sh 'DOCKER_BUILDKIT=1 docker build -f Web/Way2Go/CMS/Way2GoCMS/Dockerfile --force-rm -t joytime-web-api .'
                        }
                    }
                }
            }
        }
        stage('Push Docker Images') {
            parallel {
                stage('Push Web Docker Image') {
                    steps {
                        withDockerRegistry(credentialsId: 'docker-hub', url: 'https://index.docker.io/v1/') {
                            sh 'docker tag joytime-web-cms consortio/joytime-web-cms:latest'
                            sh 'docker push consortio/joytime-web-cms:latest'
                        }
                    }
                }
                stage('Push CMS Docker Image') {
                    steps {
                        withDockerRegistry(credentialsId: 'docker-hub', url: 'https://index.docker.io/v1/') {
                            sh 'docker tag way2go-dev-cms consortio/way2go-dev-cms:latest'
                            sh 'docker push consortio/way2go-dev-cms:latest'
                        }
                    }
                }
            }
        }
        stage('Deploy Docker Images') {
            steps {
                script {
                    sh './deploy.sh'
                }
            }
        }
    }
}