## UploadImage-S3-CloudFront-AWS

This document outlines how to build, deploy, and run the `UploadImage-S3-CloudFront-AWS` application using Docker and potentially Docker Compose.

### Getting Started

```bash
git remote add origin https://gitlab.com/phamtiendat246/uploadimage-s3-cloud-front-aws.git
git branch -M main
git push -uf origin main
```

### Building and Pushing the Docker Image

Use the following commands to build and push the Docker image to the GitLab Container Registry:

```bash
docker login registry.gitlab.com
docker build -t registry.gitlab.com/phamtiendat246/uploadimage-s3-cloud-front-aws:latest .
docker push registry.gitlab.com/phamtiendat246/uploadimage-s3-cloud-front-aws:latest
```

### Running the Docker Container (Local Development)

The following command runs the Docker container locally, mapping port 8080 inside the container to port 8082 on your host machine.  Crucially, it also sets the necessary AWS environment variables.

**Important:** Replace `your_access_key`, `your_secret_key`, and `your_region` with your actual AWS credentials and region.

```bash
docker run -d --name s3uploader \
  -p 8082:8080 \
  -e AWS_ACCESS_KEY_ID=your_access_key \
  -e AWS_SECRET_ACCESS_KEY=your_secret_key \
  -e AWS_REGION="ap-southeast-7"  \
  registry.gitlab.com/phamtiendat246/uploadimage-s3-cloud-front-aws:latest
```

**Explanation of Options:**

*   `-d`: Runs the container in detached mode (in the background).
*   `--name s3uploader`: Assigns the name `s3uploader` to the container for easier management.
*   `-p 8082:8080`: Maps port 8080 inside the container to port 8082 on the host. You'll access the application on `http://localhost:8082`.
*   `-e AWS_ACCESS_KEY_ID=your_access_key`: Sets the AWS access key ID as an environment variable.
*   `-e AWS_SECRET_ACCESS_KEY=your_secret_key`: Sets the AWS secret access key as an environment variable.
*   `-e AWS_REGION="ap-southeast-1"`: Sets the AWS region as an environment variable.  Ensure you're using the correct region.
*   `registry.gitlab.com/phamtiendat246/uploadimage-s3-cloud-front-aws:latest`: Specifies the Docker image to use.

**Alternative - Supplying AWS Credentials via AWS Profile**

If you prefer to use an AWS profile, you'll need to ensure the AWS CLI is configured correctly on the host machine. You can use environment variables this way:

```bash
docker run -d --name s3uploader \
  -p 8082:8080 \
  -e AWS_PROFILE=your_aws_profile  \
  -e AWS_REGION="ap-southeast-7" \
  registry.gitlab.com/phamtiendat246/uploadimage-s3-cloud-front-aws:latest
```

### Deployment to a Production Environment

For production deployments, consider using Docker Compose for orchestration and configuration management.  Here's a basic `docker-compose.yml` example:

```yaml
version: "3.9"
services:
  s3uploader:
    image: registry.gitlab.com/phamtiendat246/uploadimage-s3-cloud-front-aws:latest
    ports:
      - "8080:8080" # Expose only internal container port
    environment:
      AWS_ACCESS_KEY_ID: ${AWS_ACCESS_KEY_ID}
      AWS_SECRET_ACCESS_KEY: ${AWS_SECRET_ACCESS_KEY}
      AWS_REGION: ${AWS_REGION:-ap-southeast-1} # Add a default region
    restart: always
```

**Key improvements with Docker Compose:**

*   **Environment Variables:** The `docker-compose.yml` uses `${AWS_ACCESS_KEY_ID}`, `${AWS_SECRET_ACCESS_KEY}`, and `${AWS_REGION}` to source the environment variables from your system.  This avoids hardcoding sensitive information in the Compose file.  Before running `docker-compose up`, you **must** set these variables in your shell.
*   **Restart Policy:** `restart: always` ensures the container restarts automatically if it crashes.
*   **Port Mapping:** `8080:8080` exposes the container port *internally*. In a production environment, you will typically use a reverse proxy like Nginx or Apache to handle external requests and forward them to the container. This adds security and allows you to manage TLS/SSL certificates.

**Deployment Steps (Production):**

1.  **Set Environment Variables:**

    ```bash
    export AWS_ACCESS_KEY_ID=your_access_key
    export AWS_SECRET_ACCESS_KEY=your_secret_key
    export AWS_REGION=ap-southeast-1
    ```

    Or, if you're using a profile:

    ```bash
    export AWS_PROFILE=your_aws_profile
    export AWS_REGION=ap-southeast-1
    ```
    Then ensure the `AWS_ACCESS_KEY_ID` and `AWS_SECRET_ACCESS_KEY` can be pulled correctly using `aws sts get-caller-identity`

2.  **Run Docker Compose:**

    ```bash
    docker-compose up -d
    ```

    This starts the container in detached mode.

3.  **Configure Reverse Proxy (Recommended):**

    Configure a reverse proxy (CloudFront) to cache and distribute your uploaded images.  This step is optional but recommended for production environments.  You only need to configure this once.

    Set up a reverse proxy (Nginx, Apache) to forward requests from your domain (e.g., `http://your-product-domain.com/upload`) to the container's internal port (8080).  Example Nginx configuration:

    ```nginx
    server {
        listen 80;
        listen [::]:80;
        server_name s3uploader.hndedu.com; # Replace this with your domain

        location / {
            proxy_pass http://localhost:8082; # Replace this with your port forwarding to the container running the application
            proxy_set_header Host $host;
            proxy_set_header X-Real-IP $remote_addr;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto $scheme;
        }

        # Redirect HTTP to HTTPS
        if ($host = your-product-domain.com) { # Replace this with your domain. Because this is a self-hosted application, you need to redirect HTTP to HTTPS
            return 301 https://$host$request_uri;
        }
    }

    server {
        listen 443 ssl http2;
        listen [::]:443 ssl http2;
        server_name s3uploader.hndedu.com; # Replace this with your domain

        # SSL configuration
        ssl_certificate /etc/letsencrypt/live/s3uploader.hndedu.com/fullchain.pem; # This is the certificate file path in order to config https
        ssl_certificate_key /etc/letsencrypt/live/s3uploader.hndedu.com/privkey.pem; # This is the private key file path in order to config https
        ssl_session_cache shared:SSL:1m;
        ssl_session_timeout 10m;
        ssl_ciphers 'EECDH+AESGCM:EDH+AESGCM:AES256+EECDH:AES256+EDH';
        ssl_prefer_server_ciphers on;

        location / {
            proxy_pass http://localhost:8082; # Replace this with your port forwarding to the container running the application
            proxy_set_header Host $host;
            proxy_set_header X-Real-IP $remote_addr;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto $scheme;
        }
    }
    ```

    *Replace `your-product-domain.com` with your actual domain.*
    And then replace `s3uploader.hndedu.com` with your actual domain, you run below commands to reload nginx configuration:
    ```bash
    sudo nginx -s reload
    ```

    You can also test the format configuration by running:
    ```bash
    sudo nginx -t
    ```

    You can check logs by running:
    ```bash
    sudo tail -f /var/log/nginx/error.log
    ```

    You can check access logs by running:
    ```bash
    sudo tail -f /var/log/nginx/access.log
    ```

    You can check error logs by running:
    ```bash
    sudo journalctl -u nginx
    ```

4.  **Access the API (Production):**

    After setting up the reverse proxy, you can access the API through your domain:

    ```
    http://your-product-domain.com/upload
    ```

### Managing the Container

*   **Stop Container:**

    ```bash
    docker stop s3uploader
    ```

*   **Remove Container:**

    ```bash
    docker rm s3uploader
    ```

*   **Remove Image:**

    ```bash
    docker rmi registry.gitlab.com/phamtiendat246/uploadimage-s3-cloud-front-aws:latest
    ```

*   **Stop and Remove Docker Compose (Development):**

    ```bash
    docker-compose down
    ```

*   **Stop and Remove Docker Compose (Production - with Volume Cleanup):**

    ```bash
    docker-compose down -v
    ```
    This will also remove any volumes associated with the deployment.

### Deploying a New Version

Before deploying a new version, always check if the existing container is running:

```bash
docker ps
```

If a container with the name `s3uploader` is running, stop and remove it:

```bash
docker stop s3uploader
docker rm s3uploader
```

Then, pull the updated image (if necessary), and rerun the `docker run` or `docker-compose up` command with the new image tag.  For Docker Compose, it usually looks like this:

```bash
docker-compose pull   # Pulls latest image versions
docker-compose up -d  # Recreates the containers with updated images
```

**Important Considerations:**

*   **Security:**  Do *not* hardcode AWS credentials in your Dockerfile or `docker-compose.yml`.  Use environment variables as shown in the examples.  For production environments, consider using more robust secrets management solutions like AWS Secrets Manager or HashiCorp Vault.
*   **IAM Roles:**  For production environments, it's generally better practice to use IAM roles assigned to the EC2 instance or container instead of storing access keys directly.  This is more secure.
*   **Error Handling and Logging:** Implement robust error handling and logging in your application.  Configure your Docker containers to send logs to a centralized logging system (e.g., AWS CloudWatch, ELK stack).
*   **Health Checks:**  Define health checks in your Dockerfile or `docker-compose.yml` so that Docker can automatically restart unhealthy containers.
*   **Versioning:** Use tagged versions of your Docker images (e.g., `v1.0.0`) instead of `latest` for better reproducibility and rollback capabilities.
*   **Networking:**  Carefully plan your networking configuration, especially in production.  Use a reverse proxy and consider using a private network for communication between your application and other services.
*   **S3 Bucket Policy:** Configure your S3 bucket policy to restrict access to authorized users and services.
*   **CloudFront Configuration:** Configure CloudFront to properly cache and distribute your uploaded images, optimize performance, and secure your content.
*   **CI/CD Pipeline:** Implement a CI/CD pipeline to automate the building, testing, and deployment of your application. GitLab CI/CD can be used for this.

This detailed guide provides a solid foundation for deploying your image uploader application to AWS using Docker. Remember to adapt the instructions and configurations to your specific needs and security requirements.
