locals {
  task_max_number = {
    dev = "1"
    preprod = "3"
    prod = "30"
  }
  task_min_number = {
    dev = "1"
    preprod = "1"
    prod = "10"
  }
  task_desired_number = {
    dev = "1"
    preprod = "2"
    prod = "15"
  }
  service_scale_in_cooldown = {
    dev = "180"
    preprod = "180"
    prod = "180"
  }
  service_scale_out_cooldown = {
    dev = "30"
    preprod = "30"
    prod = "30"
  }
  service_scaling_target = {
    dev = "85"
    preprod = "85"
    prod = "85"
  }
}

