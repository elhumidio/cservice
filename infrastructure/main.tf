provider "aws" {
  region = var.region
}

terraform {
  backend "s3" {
  }
}

module "ecs-service" {
  source  = "terraform-registry.stepstone.tools/turi-contractservice__ibb/ecs-service/aws"
  version = ">= 2.4.3, < 3.0.0"

  service_name                = var.service_name
  desired_count               = var.desired_count
  ecs_cluster_id              = var.ecs_cluster_id
  aws_lb_name                 = var.aws_lb_name
  vpc_id                      = var.vpc_id
  hosted_zone_id              = var.hosted_zone_id
  iam_role_name               = var.iam_role_name
  target_group_name           = var.target_group_name
  health_check_path           = "/ping"
  health_check_matcher        = "200"
  enable_service_auto_scaling = var.enable_service_auto_scaling
}

variable "service_name" {}
variable "region" {}
variable "vpc_id" {}
variable "hosted_zone_id" {}
variable "aws_lb_name" {}
variable "desired_count" {}
variable "ecs_cluster_id" {}
variable "iam_role_name" {}
variable "target_group_name" {}
variable "environment" {}
variable "namespace" {}
variable "team_name" {}
variable "enable_service_auto_scaling" {
  type = bool
}
