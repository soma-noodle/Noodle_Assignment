namespace Noodle_Assignment.Controllers
{
    [ApiController]
    [Route("excercise")]
    public class ExcerciseController : ControllerBase
    {
        #region Readonly Variables

        private readonly ILogger<ExcerciseController> _logger;
        private readonly IGraphQLService _graphQLService;
        private readonly IDummyExerciseServie _dummyExerciseServie;
        private readonly ICreateService _createService;
        private readonly IUpdateGroupService _updateGroupService;
        private readonly IStateMachineService _stateMachineService;
        private readonly ICheckOutService _checkOutService;
        private readonly ICartMergingService _cartMergingService;
        private readonly IInStoreService _inStoreService;
        private readonly IMeService _meService;
        private readonly IProductSelectionsService _productSelectionsService;
        private readonly ISearchService _searchService;
        private readonly IPagedQueryService _pagedQueryService;
        private readonly ICustomTypesService _customTypesService;
        private readonly ICustomObjectsService _customObjectsService;
        private readonly IApiExtensionService _apiExtensionService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IErrorHandlingService _errorHandlingService;

        #endregion

        #region Constructor 

        public ExcerciseController(ILogger<ExcerciseController> logger,
            IGraphQLService graphQLService,
            ICreateService createService,
            IUpdateGroupService updateGroupService,
            IStateMachineService stateMachineService,
            ICheckOutService checkOutService,
            ICartMergingService cartMergingService,
            IInStoreService inStoreService,
            IMeService meService,
            IProductSelectionsService productSelectionsService,
            ISearchService searchService,
            IPagedQueryService pagedQueryService,
            ICustomTypesService customTypesService,
            ICustomObjectsService customObjectsService,
            IApiExtensionService apiExtensionService,
            ISubscriptionService subscriptionService,
            IErrorHandlingService errorHandlingService,
            IDummyExerciseServie dummyExerciseServie)
        {
            _logger = logger;
            _graphQLService = graphQLService;
            _createService = createService;
            _updateGroupService = updateGroupService;
            _stateMachineService = stateMachineService;
            _checkOutService = checkOutService;
            _cartMergingService = cartMergingService;
            _inStoreService = inStoreService;
            _meService = meService;
            _productSelectionsService = productSelectionsService;
            _searchService = searchService;
            _pagedQueryService = pagedQueryService;
            _customTypesService = customTypesService;
            _customObjectsService = customObjectsService;
            _apiExtensionService = apiExtensionService;
            _subscriptionService = subscriptionService;
            _errorHandlingService = errorHandlingService;
            _dummyExerciseServie = dummyExerciseServie;
        }

        #endregion

        #region Dummy Exercise        

        [HttpGet("dummy-execute")]
        public async Task<string> DummyExecute()
        {
            return await _dummyExerciseServie.ExecuteAsync();
        }

        #endregion

        #region Task02a_CREATE

        // Sample JSON structure

        //{  
        //  "customerNumber":"7",
        //  "email": "soma7@example.com",
        //  "password": "password",
        //  "firstName": "SSS7",
        //  "lastName": "PPP7",  
        //  "defaultShippingAddress":0,
        //  "defaultBillingAddress":0
        //}

        [HttpPost("create-customer-with-email-confirmation")]
        public async Task<string> CreateCustomerWithEmailConfirmation([FromBody] CustomerModel customerModel)
        {
            return await _createService.ExecuteAsync(customerModel);
        }

        #endregion

        #region  Task02b_UPDATE_Group

        // Sample JSON structure

        //{
        // "customerId":"9215ed81-83a8-4741-b8cd-2a8f51dbce1a",
        // "customerGroupId":"84dea057-6906-42a7-8c37-a72ead3c4b86"
        //}

        [HttpPost("set-customergroup-for-the-customer")]
        public async Task<string> SetCustomerGroupForTheCustomer([FromBody] UpdateServiceModel model)
        {
            return await _updateGroupService.ExecuteAsync(model);
        }

        #endregion

        #region Task04a_STATEMACHINE

        // Sample JSON structure

        //{  
        //  "stateOrder1_Key":"OrderPacked1",
        //  "stateOrder1_Name": "Order Packed1",
        //  "stateOrder2_Key":"OrderShipped2",
        //  "stateOrder2_Name": "Order Shipped2",
        //}

        [HttpPost("create-and-update-state-transitions")]
        public async Task<string> CreateAndUpdateStateTransitions([FromBody] StateMachineModel model)
        {
            return await _stateMachineService.ExecuteAsync(model);
        }

        #endregion

        #region Task04b_CHECKOUT

        [HttpPost("cart-checkout-process")]
        public async Task<string> CartCheckoutProcess(CheckoutModel model)
        {
            return await _checkOutService.ExecuteAsync(model);
        }

        #endregion

        #region Task04c_CART_MERGING

        [HttpPost("merge-cart")]
        public async Task<string> MergeCart([FromBody] CartMergeModel cartMergeModel)
        {
            return await _cartMergingService.ExecuteAsync(cartMergeModel);
        }

        #endregion

        #region Task05A_INSTORE

        [HttpPost("create-cart-in-store")]
        public async Task<string> CreateCartInStore([FromBody] InStoreModel inStoreModel)
        {
            return await _inStoreService.ExecuteAsync(inStoreModel);
        }

        #endregion

        #region Task05B_Me

        [HttpPost("my-profile")]
        public async Task<string> MyProfile([FromBody] MeClientModel meClient)
        {
            return await _meService.ExecuteAsync(meClient);
        }

        #endregion

        #region Task05c_PRODUCTSELECTIONS

        [HttpPost("product-selections")]
        public async Task<string> ProductSelections([FromBody] ProductSelectionModel productSelectionModel)
        {
            return await _productSelectionsService.ExecuteAsync(productSelectionModel);
        }

        #endregion

        #region Task06a_SEARCH

        [HttpPost("search")]
        public async Task<string> Search()
        {
            return await _searchService.ExecuteAsync();
        }

        #endregion

        #region Task06b_PAGEDQUERY

        [HttpPost("get-products-sort-by-id")]
        public async Task<string> GetProductsSortById()
        {
            return await _pagedQueryService.ExecuteAsync();
        }

        #endregion

        #region Task06c_GRAPHQL

        [HttpGet("get-customers-email-using-graphql-query")]
        public async Task<List<string>> GetCustomersEmailUsingGraphQLQuery()
        {
            return await _graphQLService.ExecuteAsync();
        }

        #endregion

        #region Task07a_CUSTOMTYPES

        [HttpPost("add-customfield-to-customer")]
        public async Task<string> AddCustomFieldToCustomer([FromBody] CustomTypeModel customTypeModel)
        {
            return await _customTypesService.ExecuteAsync(customTypeModel);
        }

        #endregion

        #region Task07b_CUSTOMOBJECTS

        [HttpPost("create-custom-object")]
        public async Task<string> CreateCustomObject()
        {
            return await _customObjectsService.ExecuteAsync();
        }

        #endregion

        #region Task07c_APIEXTENSION

        [HttpPost("create-api-extension-service")]
        public async Task<string> CreateAPIExtensionService()
        {
            return await _apiExtensionService.ExecuteAsync();
        }

        #endregion

        #region Task08a_SUBSCRIPTION

        [HttpPost("create-subscription")]
        public async Task<string> CreateSubscription()
        {
            return await _subscriptionService.ExecuteAsync();
        }

        #endregion

        #region Task09a_ERROR_HANDLING

        [HttpGet("error-handling-execute")]
        public async Task<string> ErrorHandlingExecute()
        {
            return await _errorHandlingService.ExecuteAsync();
        }

        #endregion
    }
}
