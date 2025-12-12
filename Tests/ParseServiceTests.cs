using Formatter.Configuration;
using Parser;

namespace Tests;

public sealed class ParseServiceTests
{
    private readonly ParseService _parseService = new();

    [Theory]
    [MemberData(nameof(GetInterfaceInputsAndOutputs))]
    private async Task TextShouldBeParsedSuccessfully(string input, string expected)
    {
        FormatConfiguration.TurnEverythingOff();

        var result = await _parseService.ParseText(input, new Config());

        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> GetInterfaceInputsAndOutputs()
    {
        yield return
        [
            """
            public class CarRentalEntity
            {
                public string Id { get; set; }
                public string UserId { get; set; }
                public string VehicleId { get; set; }
                public VehicleEntity? Vehicle { get; set; }
                public string? DropOffStoreId { get; set; }
                public StoreEntity? DropOffStore { get; set; }
                public List<CarRentalCoverageEntity>? Coverages { get; set; }
                public DeliveryEntity? Delivery { get; set; }
                public List<CarRentalTransactionEntity>? Transactions { get; set; }
                public List<CarRentalTimelineEntity>? Timeline { get; set; }
                public DateTime? PickupDate { get; set; }
                public DateTime? DropoffDate { get; set; }
                public int? Rating { get; set; }
                public string? Status { get; set; }
            }
            """,
            """
            export interface CarRentalEntity {
              id:  string;
              userId:  string;
              vehicleId:  string;
              vehicle:  VehicleEntity;
              dropOffStoreId:  string;
              dropOffStore:  StoreEntity;
              coverages:  CarRentalCoverageEntity[];
              delivery:  DeliveryEntity;
              transactions:  CarRentalTransactionEntity[];
              timeline:  CarRentalTimelineEntity[];
              pickupDate:  Date;
              dropoffDate:  Date;
              rating:  number;
              status:  string;
            }
            """
        ];

        yield return
        [
            """
            public class CarRentalEntity
            {
                public string Id { get; set; }
                public string UserId { get; set; }

                // Rental details
                public string VehicleId { get; set; }
                public VehicleEntity? Vehicle { get; set; }

                public string? PickupStoreId { get; set; }
                public StoreEntity? PickupStore { get; set; }

                public string? DropOffStoreId { get; set; }
                public StoreEntity? DropOffStore { get; set; }

                // Dates
                public DateTime? PickupDate { get; set; }
                public DateTime? DropoffDate { get; set; }
                public DateTime? CreatedAt { get; set; }
                public DateTime? ModifiedAt { get; set; }
                public DateTime? CancelledAt { get; set; }
                public DateTime? CompletedAt { get; set; }

                // Status tracking
                public RentalStatus Status { get; set; }
                public List<CarRentalTimelineEntity>? Timeline { get; set; }

                // Financials
                public decimal? BaseDailyRate { get; set; }
                public decimal? TotalBaseCost { get; set; }
                public decimal? Taxes { get; set; }
                public decimal? Fees { get; set; }
                public decimal? DiscountAmount { get; set; }
                public List<CarRentalChargeLineItemEntity>? ChargeBreakdown { get; set; }

                // Add-on stuff
                public List<CarRentalCoverageEntity>? Coverages { get; set; }
                public List<AddonEntity>? Addons { get; set; }

                // Delivery
                public DeliveryEntity? Delivery { get; set; }

                // Transactions (refunds, holds, releases, etc.)
                public List<CarRentalTransactionEntity>? Transactions { get; set; }

                // User feedback
                public int? Rating { get; set; }
                public string? FeedbackText { get; set; }
                public List<string>? ImageUrls { get; set; }

                // Flags
                public bool IsLateReturn { get; set; }
                public bool IsPremiumCustomer { get; set; }
                public bool FraudCheckPassed { get; set; }

                // Audit + logs
                public string? CreatedBy { get; set; }
                public string? ModifiedBy { get; set; }
                public List<AuditLogEntry>? AuditLogs { get; set; }

                // Random metadata
                public Dictionary<List<string>, string>? Metadata { get; set; }
            }

            public record UserModel(
                Guid Id,
                string FirstName,
                string LastName,
                string Email,
                DateTime CreatedAt
            ){}

            public enum RentalStatus
            {
                Pending,
                Confirmed,
                VehicleAssigned,
                ReadyForPickup,
                InProgress,
                Completed,
                Cancelled,
                FailedPayment
            }

            public class CarRentalChargeLineItemEntity
            {
                public string Code { get; set; }
                public string Description { get; set; }
                public decimal Amount { get; set; }
            }

            public class AddonEntity
            {
                public string Id { get; set; }
                public string Name { get; set; }
                public decimal PricePerDay { get; set; }
            }

            public class AuditLogEntry
            {
                public DateTime Timestamp { get; set; }
                public string Action { get; set; }
                public string? PerformedBy { get; set; }
                public string? Notes { get; set; }
            }
            """,
            """
            export interface CarRentalEntity {
              id:  string;
              userId:  string;
              vehicleId:  string;
              vehicle:  VehicleEntity;
              pickupStoreId:  string;
              pickupStore:  StoreEntity;
              dropOffStoreId:  string;
              dropOffStore:  StoreEntity;
              pickupDate:  Date;
              dropoffDate:  Date;
              createdAt:  Date;
              modifiedAt:  Date;
              cancelledAt:  Date;
              completedAt:  Date;
              status:  RentalStatus;
              timeline:  CarRentalTimelineEntity[];
              baseDailyRate:  number;
              totalBaseCost:  number;
              taxes:  number;
              fees:  number;
              discountAmount:  number;
              chargeBreakdown:  CarRentalChargeLineItemEntity[];
              coverages:  CarRentalCoverageEntity[];
              addons:  AddonEntity[];
              delivery:  DeliveryEntity;
              transactions:  CarRentalTransactionEntity[];
              rating:  number;
              feedbackText:  string;
              imageUrls:  string[];
              isLateReturn:  boolean;
              isPremiumCustomer:  boolean;
              fraudCheckPassed:  boolean;
              createdBy:  string;
              modifiedBy:  string;
              auditLogs:  AuditLogEntry[];
              metadata:  Map<string[],string>;
            }

            export interface UserModel {
              id:  string;
              firstName:  string;
              lastName:  string;
              email:  string;
              createdAt:  Date;
            }

            export interface CarRentalChargeLineItemEntity {
              code:  string;
              description:  string;
              amount:  number;
            }

            export interface AddonEntity {
              id:  string;
              name:  string;
              pricePerDay:  number;
            }

            export interface AuditLogEntry {
              timestamp:  Date;
              action:  string;
              performedBy:  string;
              notes:  string;
            }
            """
        ];

        yield return
        [
            """
            public record ProjectModel(
                Guid Id,
                string Name,
                string Description,
                ProjectStatus Status,
                PriorityLevel Priority,
                UserInfo Owner,
                IReadOnlyList<UserInfo> Members,
                IReadOnlyList<TaskItem> Tasks,
                IReadOnlyList<Tag> Tags,
                DateTime CreatedAt,
                DateTime UpdatedAt,
                DateTime? DueDate,
                BudgetInfo Budget,
                IReadOnlyDictionary<string, string> Metadata,
                IReadOnlyList<Comment> Comments,
                IReadOnlyList<Attachment> Attachments
            ){}
            """,
            """
            export interface ProjectModel {
              id:  string;
              name:  string;
              description:  string;
              status:  ProjectStatus;
              priority:  PriorityLevel;
              owner:  UserInfo;
              members:  IReadOnlyList<UserInfo>;
              tasks:  IReadOnlyList<TaskItem>;
              tags:  IReadOnlyList<Tag>;
              createdAt:  Date;
              updatedAt:  Date;
              dueDate:  Date;
              budget:  BudgetInfo;
              metadata:  IReadOnlyDictionary<string,string>;
              comments:  IReadOnlyList<Comment>;
              attachments:  IReadOnlyList<Attachment>;
            }
            """
        ];

        yield return
        [
            """
            public class TestScratch
            {
                public Dictionary<string,Dictionary<string, Dictionary<Dictionary<string, Dictionary<string, string>>, stringz>>>? Metadata { get; set; }
                public Dictionary<Dictionary<string, Dictionary<string, string>>, string>? MetadataV2 { get; set; }
            }
            """,
            """
            export interface TestScratch {
              metadata:  Record<string,Record<string,Map<Record<string,Record<string,string>>,stringz>>>;
              metadataV2:  Map<Record<string,Record<string,string>>,string>;
            }
            """
        ];

        yield return
        [
            """
            public class UserDto
            {
                public Guid Id { get; set; }
                public string Username { get; set; }
                public string Email { get; set; }
                public DateTime CreatedAt { get; set; }
            }
            """,
            """
            export interface UserDto {
              id:  string;
              username:  string;
              email:  string;
              createdAt:  Date;
            }
            """
        ];

        yield return
        [
            """
            public class ProductDto
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string Description { get; set; }
                public decimal Price { get; set; }
                public int Stock { get; set; }
            }
            """,
            """
            export interface ProductDto {
              id:  string;
              name:  string;
              description:  string;
              price:  number;
              stock:  number;
            }
            """
        ];

        yield return
        [
            """
            public class OrderDto
            {
                public Guid Id { get; set; }
                public Guid UserId { get; set; }
                public DateTime CreatedAt { get; set; }
                public List<OrderItemDto> Items { get; set; }
                public decimal Total { get; set; }
            }
            """,
            """
            export interface OrderDto {
              id:  string;
              userId:  string;
              createdAt:  Date;
              items:  OrderItemDto[];
              total:  number;
            }
            """
        ];

        yield return
        [
            """
            public class LoginResponseDto
            {
                public string Token { get; set; }
                public DateTime ExpiresAt { get; set; }
            }
            """,
            """
            export interface LoginResponseDto {
              token:  string;
              expiresAt:  Date;
            }
            """
        ];

        yield return
        [
            """
            public class ErrorDto
            {
                public string Code { get; set; }
                public string Message { get; set; }
                public Dictionary<string, string[]>? Validation { get; set; }
            }
            """,
            """
            export interface ErrorDto {
              code:  string;
              message:  string;
              validation:  Record<string,string[]>;
            }
            """
        ];

        yield return
        [
            """
            public class CustomerDto
            {
                public Guid Id { get; set; }
                public string FullName { get; set; }
                public string Email { get; set; }
                public Address BillingAddress { get; set; }
                public Address? ShippingAddress { get; set; }
                public DateTime RegisteredAt { get; set; }
            }
            
            public class CatalogItemDto
            {
                public Guid Id { get; set; }
                public string Sku { get; set; }
                public string Name { get; set; }
                public Money Price { get; set; }
                public List<string>? Tags { get; set; }
                public GeoPoint? WarehouseLocation { get; set; }
            }
            
            public class ShipmentDto
            {
                public Guid Id { get; set; }
                public Guid OrderId { get; set; }
                public ShipmentStatus Status { get; set; }
                public Address Destination { get; set; }
                public GeoPoint? TrackingLocation { get; set; }
                public DateTime? DeliveredAt { get; set; }
            }
            
            public class PaymentDto
            {
                public Guid Id { get; set; }
                public Guid OrderId { get; set; }
                public Money Amount { get; set; }
                public PaymentMethod Method { get; set; }
                public DateTime Timestamp { get; set; }
                public string? TransactionReference { get; set; }
            }
            
            public class OrderSummaryDto
            {
                public Guid Id { get; set; }
                public CustomerDto Customer { get; set; }
                public List<OrderLineDto> Lines { get; set; } = new();
                public Money Total { get; set; }
                public ShipmentStatus ShipmentStatus { get; set; }
                public DateTime CreatedAt { get; set; }
            }
            
            public class OrderLineDto
            {
                public Guid CatalogItemId { get; set; }
                public int Quantity { get; set; }
                public Money UnitPrice { get; set; }
                public decimal DiscountPercent { get; set; }
            }
            
            public class AuthTokenDto
            {
                public string AccessToken { get; set; }
                public string RefreshToken { get; set; }
                public DateTime ExpiresAt { get; set; }
            }
            """,
            """
            export interface CustomerDto {
              id:  string;
              fullName:  string;
              email:  string;
              billingAddress:  Address;
              shippingAddress:  Address;
              registeredAt:  Date;
            }
            
            export interface CatalogItemDto {
              id:  string;
              sku:  string;
              name:  string;
              price:  Money;
              tags:  string[];
              warehouseLocation:  GeoPoint;
            }
            
            export interface ShipmentDto {
              id:  string;
              orderId:  string;
              status:  ShipmentStatus;
              destination:  Address;
              trackingLocation:  GeoPoint;
              deliveredAt:  Date;
            }
            
            export interface PaymentDto {
              id:  string;
              orderId:  string;
              amount:  Money;
              method:  PaymentMethod;
              timestamp:  Date;
              transactionReference:  string;
            }
            
            export interface OrderSummaryDto {
              id:  string;
              customer:  CustomerDto;
              lines:  OrderLineDto[];
              total:  Money;
              shipmentStatus:  ShipmentStatus;
              createdAt:  Date;
            }
            
            export interface OrderLineDto {
              catalogItemId:  string;
              quantity:  number;
              unitPrice:  Money;
              discountPercent:  number;
            }
            
            export interface AuthTokenDto {
              accessToken:  string;
              refreshToken:  string;
              expiresAt:  Date;
            }
            """
        ];

        yield return
        [
            """
            public class Arrays {
              public List<DateTime> Dates { get; set; } = new();
              public List<int> Ints { get; set; } = new();
              public List<string> Strings { get; set; } = new();
              public List<Dictionary<string, string>> Dictionaries { get; set; } = new();
            }
            """,
            """
            export interface Arrays {
              dates:  Date[];
              ints:  number[];
              strings:  string[];
              dictionaries:  Record<string,string>[];
            }
            """
        ];

        yield return
        [
            """
            public class TestScratch2
            {
                public List<List<Dictionary<Dictionary<string, Dictionary<string, string>>, string>>>? Metadata { get; set; }
                public List<Dictionary<Dictionary<string, Dictionary<string, string>>, string>>? MetadataV2 { get; set; }
                public List<List<List<List<int>>>> test1;
                public List<Dictionary<List<List<int>>, List<List<int>>>> test2;
                public List<Dictionary<List<List<List<Dictionary<List<List<int>>, List<List<int>>>>>>, List<List<int>>>> test3;
                public Dictionary<List<Dictionary<List<List<List<Dictionary<List<List<int>>, List<List<int>>>>>>, List<List<int[]>>>>,Test> test4;
                public Dictionary<int[], int[]> testz;
            }
            """,
            """
            export interface TestScratch2 {
              metadata:  Map<Record<string,Record<string,string>>,string>[][];
              metadataV2:  Map<Record<string,Record<string,string>>,string>[];
              test1:  number[][][][];
              test2:  Map<number[][],number[][]>[];
              test3:  Map<Map<number[][],number[][]>[][][],number[][]>[];
              test4:  Map<Map<Map<number[][],number[][]>[][][],number[][][]>[],Test>;
              testz:  Map<number[],number[]>;
            }
            """
        ];
    }
}
