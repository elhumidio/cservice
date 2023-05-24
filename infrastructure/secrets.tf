# provider "vault" {
#   address         = "https://vault-${var.environment}.stepstone.tools/"
#   skip_tls_verify = true

#   auth_login {
#     path   = "auth/aws/login"
#     method = "aws"
#     parameters = {
#       role         = "app-deployment-user-role"
#       sts_endpoint = "https://sts.amazonaws.com"
#       sts_region   = "us-east-1"
#     }
#   }
# }

# module "secrets" {
#   source = "git::ssh://git@stash.stepstone.com:7999/tfm/release-zero-secret.git?ref=v2.3.0"

#   environment                     = var.environment
#   team_name                       = var.team_name
#   application_namespace           = var.namespace
#   application_service_name        = var.service_name
#   ecs_cluster_name                = var.ecs_cluster_id
#   vault_aws_role_owner_arn        = module.ecs-service.task_role_arn
#   vault_aws_role_consume_policies = []
# }

# output "vault_policy_consumer_names" {
#   value = module.secrets.vault_policy_consumer_names
# }
