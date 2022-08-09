# This specifies a secret in Vault to watch. This may be specified multiple
# times to watch multiple secrets, and the bottom-most secret takes
# precedence, should any values overlap. Secret blocks without the path
# defined are meaningless and are discarded. If secret names conflict with
# prefix names, secret names will take precedence.

# This tells Envconsul to convert environment variable keys to uppercase (which
# is more common and a bit more standard). Default is set to false.
# upcase = false

# secret {
#   # This tells Envconsul to not add a path prefix to the environment variable name.
#   no_prefix = true
#   path = "service/data/1cd/turi/contractservice/<secret_key>"
#   
#	key {
#     name = "value"
#     format = "<ENVIRONMENT VARIABLE NAME IN CONTAINER>"
#   }
#
#	# If your are the Owner of this secret (your app is a Resource Owner), please remember to support both keys (in case of a rotation)
#	key {
#     name = "pending"
#     format = "<ENVIRONMENT VARIABLE NAME IN CONTAINER>_pending"
#   }
# }
